using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public CardObject[] PlayerLanes = new CardObject[3];

    [Header("Rival Data")]
    public List<CardObject> RivalHand = new List<CardObject>();
    public List<CardObject> RivalDeck = new List<CardObject>();
    public CardObject[] RivalLanes = new CardObject[3];

    [Header("Scene References")]
    [SerializeField] private Transform playerHandTransform;
    [SerializeField] private GameObject cardPrefab;

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

    public void SelectCardForPlay(CardObject _card, GameObject _cardGameObject)
    {
        // If there is a card selected, deselect it
        if(selectedCard != null) selectedCardObject.GetComponent<CardInstance>().DeselectCard();

        selectedCard = _card;
        selectedCardObject = _cardGameObject;
    }

    public bool PlayCard(int _lane)
    {
        if(selectedCard == null)
        {
            return false;
        }

        if(selectedCard.Type == CardType.Creature)
        {
            if(PlayerLanes[_lane] == null)
            {
                PlayerLanes[_lane] = selectedCard;

                selectedCard.Behaviour?.Initialize(_lane, 0);

                RemoveCard(true, selectedCard);

                selectedCard = null;
                selectedCardObject = null;

                return true;
            }
        }
        else if(selectedCard.Type == CardType.Spell)
        {
            selectedCard.Behaviour.Initialize(_lane, 0);
            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;
            
            return true;
        }
        

        // If the lane is occupied, return false to notify
        return false;
    }
}
