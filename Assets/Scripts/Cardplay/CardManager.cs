using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    #region Singleton Definition
    public static CardManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Gameplay Definitions")]
    [SerializeField] private int handCapacity = 4;
    [SerializeField] private int deckCapacity = 10;

    [Header("Player Data")]
    public List<GameObject> PlayerHand = new List<GameObject>();
    public List<CardObject> PlayerDeck = new List<CardObject>();
    public CardObject[] PlayerCreatureLanes = new CardObject[3];
    public CardObject[] PlayerStructureLanes = new CardObject[3];

    [Header("Rival Data")]
    public List<CardObject> RivalHand = new List<CardObject>();
    public List<CardObject> RivalDeck = new List<CardObject>();
    public CardObject[] RivalCreatureLanes = new CardObject[3];
    public CardObject[] RivalStructureLanes = new CardObject[3];

    [Header("Scene References")]
    [SerializeField] private Transform playerHandTransform;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Image[] cardHolderCards;

    [SerializeField] private List<Transform> playerLaneHologramMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalLaneHologramMarkers = new List<Transform>();

    private GameObject[] playerLaneHologramObjects = new GameObject[3];
    private GameObject[] rivalLaneHologramObjects = new GameObject[3];

    [SerializeField] private List<Transform> playerStructureLaneHologramMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalStructureLaneHologramMarkers = new List<Transform>();

    private GameObject[] playerStructureLaneHologramObjects = new GameObject[3];
    private GameObject[] rivalStructureLaneHologramObjects = new GameObject[3];

    private CardObject selectedCard;
    private GameObject selectedCardObject;

    public bool DrawCard(bool _player)
    {
        if (_player && PlayerHand.Count <= handCapacity)
        {
            // Select a card to draw
            int _rand = Random.Range(0, PlayerDeck.Count - 1);

            // Create the object for the user to interact with
            GameObject _cardObject = Instantiate(cardPrefab, playerHandTransform);
            _cardObject.GetComponentInChildren<CardInstance>().AssignCard(PlayerDeck[_rand]);

            PlayerHand.Add(_cardObject);

            // Remove card from deck
            PlayerDeck.RemoveAt(_rand);

            return true;
        }
        else if(RivalHand.Count <= handCapacity)
        {
            // Select a card to draw
            int _rand = Random.Range(0, RivalDeck.Count - 1);
            RivalHand.Add(RivalDeck[_rand]);

            // Remove card from deck
            RivalDeck.RemoveAt(_rand);

            return true;
        }

        // If not enough capacity, return false to notify
        return false;
    }

    #region Deck Management
    public bool AddCardToDeck(bool _player, CardObject _card)
    {
        if(_player && PlayerDeck.Count >= deckCapacity) return false;
        else if(!_player && RivalDeck.Count >= deckCapacity) return false;

        if(_player) PlayerDeck.Add(_card);
        else RivalDeck.Add(_card);

        return true;
    }

    public bool RemoveCard(bool _player, CardObject _card)
    {
        if(_player)
        {
            for (int i = 0; i < PlayerHand.Count; i++)
            {
                if(PlayerHand[i].GetComponentInChildren<CardInstance>().Card == _card)
                {
                    Destroy(PlayerHand[i]);
                    PlayerHand.RemoveAt(i);

                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < RivalHand.Count; i++)
            {
                if(RivalHand[i] == _card)
                {
                    RivalHand.RemoveAt(i);

                    return true;
                }
            }
        }

        // If the card was not present on the hand, return false to notify
        return false;
    }

    #endregion

    #region Card Play
    public void SelectCardForPlay(CardObject _card, GameObject _cardGameObject)
    {
        // If there is a card selected, deselect it
        if(selectedCard != null) selectedCardObject.GetComponent<CardInstance>().DeselectCard();

        selectedCard = _card;
        selectedCardObject = _cardGameObject;
    }

    public void DeselectCardForPlay()
    {
        selectedCard = null;
        selectedCardObject = null;
    }

    public bool PlayCard(int _lane)
    {
        if(selectedCard == null)
        {
            return false;
        }

        cardHolderCards[_lane].gameObject.SetActive(true);
        cardHolderCards[_lane].sprite = selectedCard.Sprite;

        if(selectedCard.Type == CardType.Creature && PlayerCreatureLanes[_lane] == null)
        {
            CardObject _cardClone = new CardObject();
            _cardClone.CardName = selectedCard.CardName;
            _cardClone.Sprite = selectedCard.Sprite;
            _cardClone.Type = selectedCard.Type;
            _cardClone.Health = selectedCard.Health;
            _cardClone.MaxHealth = selectedCard.MaxHealth;
            _cardClone.Damage = selectedCard.Damage;

            PlayerCreatureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            playerLaneHologramObjects[_lane] = Instantiate(selectedCard.Hologram, playerLaneHologramMarkers[_lane]);
            selectedCard.Behaviour?.Initialize(_lane, 1);

            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;

            return true;
            
        }
        else if(selectedCard.Type == CardType.Spell)
        {
            selectedCard.Behaviour.Initialize(_lane, 1);
            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;
            
            return true;
        }
        else if(selectedCard.Type == CardType.Structure && PlayerStructureLanes[_lane] == null)
        {
            CardObject _cardClone = new CardObject();
            _cardClone.CardName = selectedCard.CardName;
            _cardClone.Sprite = selectedCard.Sprite;
            _cardClone.Type = selectedCard.Type;
            _cardClone.Health = selectedCard.Health;
            _cardClone.MaxHealth = selectedCard.MaxHealth;

            PlayerStructureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            playerStructureLaneHologramObjects[_lane] = Instantiate(selectedCard.Hologram, playerStructureLaneHologramMarkers[_lane]);
            selectedCard.Behaviour.Initialize(_lane, 0);
            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;
            
            return true;
        }
        
        // If the lane is occupied, return false to notify
        return false;
    }

    #endregion

    public CardObject GetCardAt(int _lane, int _row)
    {
        switch(_row)
        {
            case 0:
                return PlayerStructureLanes[_lane];
                break;

            case 1:
                return PlayerCreatureLanes[_lane];
                break;
        }

        return null;
    }
}
