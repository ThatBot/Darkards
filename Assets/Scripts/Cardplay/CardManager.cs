using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    #region Variable Definition

    [Header("Gameplay Definitions")]
    [SerializeField] private int handCapacity = 4;
    [SerializeField] private int deckCapacity = 10;
    [SerializeField] private int actionCapacity = 3;
    public int ActionsLeft = 3;
    public int RivalActionsLeft = 3;
    public bool PlayerPriority = true;
    [SerializeField] private CardObject[] tokenCards;
    public bool InAnimation = false;

    [Header("Player Data")]
    public DeckObject PlayerDeckObject;
    public List<GameObject> PlayerHand = new List<GameObject>();
    public List<CardObject> PlayerDeck = new List<CardObject>();
    public CardObject[] PlayerCreatureLanes = new CardObject[3];
    public CardObject[] PlayerStructureLanes = new CardObject[3];

    [Header("Rival Data")]
    public DeckObject[] AvailableDecks;
    public List<CardObject> RivalHand = new List<CardObject>();
    public List<CardObject> RivalDeck = new List<CardObject>();
    public CardObject[] RivalCreatureLanes = new CardObject[3];
    public CardObject[] RivalStructureLanes = new CardObject[3];
    private RivalAI rivalAI = new RivalAI();
    private bool isOnAction = false;

    [Header("Scene References")]
    [SerializeField] private CameraController cameraCont;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    [Space]

    [SerializeField] private Transform playerHandTransform;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Image[] cardHolderCards;

    [Space]

    [SerializeField] private List<Transform> playerLaneHologramMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalLaneHologramMarkers = new List<Transform>();

    private GameObject[] playerLaneHologramObjects = new GameObject[3];
    private GameObject[] rivalLaneHologramObjects = new GameObject[3];

    [SerializeField] private List<Transform> playerStructureLaneHologramMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalStructureLaneHologramMarkers = new List<Transform>();

    private GameObject[] playerStructureLaneHologramObjects = new GameObject[3];
    private GameObject[] rivalStructureLaneHologramObjects = new GameObject[3];

    [Space]

    [SerializeField] private List<Transform> playerLaneSelectedMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalLaneSelectedMarkers = new List<Transform>();
    [SerializeField] private List<Transform> playerStructureLaneSelectedMarkers = new List<Transform>();
    [SerializeField] private List<Transform> rivalStructureLaneSelectedMarkers = new List<Transform>();

    [Space]

    [SerializeField] private ParticleSystem[] playerLaneAttackEffects = new ParticleSystem[3];
    [SerializeField] private ParticleSystem[] rivalLaneAttackEffects = new ParticleSystem[3];

    [SerializeField] private ParticleSystem[] playerLaneHurtEffects = new ParticleSystem[3];
    [SerializeField] private ParticleSystem[] rivalLaneHurtEffects = new ParticleSystem[3];
    [SerializeField] private ParticleSystem[] playerStructureLaneHurtEffects = new ParticleSystem[3];
    [SerializeField] private ParticleSystem[] rivalStructureLaneHurtEffects = new ParticleSystem[3];

    private CardObject selectedCard;
    private GameObject selectedCardObject;

    private bool onIntro = true;
    private bool coinsAdded = false;
    private bool victoryAdded = false;
    private bool lostAdded = false;
    private int startingPlayer = 0;

    #endregion

    private void Start()
    {
        startingPlayer = IntroController.Instance.InitiateIntro();
    }

    private void Update()
    {
        if(onIntro) return;

        HandleWinCondition();

        // Switch turns when actions run out
        if(ActionsLeft <= 0 || RivalActionsLeft <= 0) 
        {
            SwitchTurn();
            return;
        }
        else if (RivalActionsLeft > 0 && !PlayerPriority && !isOnAction) 
        {
            isOnAction = true;
            rivalAI.ProcessAction();
        }
    }

    public void PopulateField()
    {
        // Populate the decks for both the player and rival
        int _rand = Random.Range(0, AvailableDecks.Length);

        for (int i = 0; i < PlayerDeckObject.Deck.Count; i++)
        {
            AddCardToDeck(true, PlayerDeckObject.Deck[i]);
        }
        
        for (int i = 0; i < AvailableDecks[_rand].Deck.Count; i++)
        {
            AddCardToDeck(false, AvailableDecks[_rand].Deck[i]);
        }

        // Obtain the first three cards, first the player and then the rival
        DrawFreeCard(true);
        DrawFreeCard(true);
        DrawFreeCard(true);

        DrawFreeCard(false);
        DrawFreeCard(false);
        DrawFreeCard(false);

        // Populate the player's structure area
        for (int i = 0; i < PlayerDeckObject.Structures.Count; i++)
        {
            CardObject _structureCard = PlayerDeckObject.Structures[i];

            CardObject _cardClone = new CardObject();
            _cardClone.CardName = _structureCard.CardName;
            _cardClone.CardDescription = _structureCard.CardDescription;
            _cardClone.Sprite = _structureCard.Sprite;
            _cardClone.Type = _structureCard.Type;
            _cardClone.Health = _structureCard.Health;
            _cardClone.MaxHealth = _structureCard.MaxHealth;
            _cardClone.Behaviour = _structureCard.Behaviour;

            PlayerStructureLanes[i] = _cardClone;

            // Spawn the card's model on the correct spot
            playerStructureLaneHologramObjects[i] = Instantiate(_structureCard.Hologram, playerStructureLaneHologramMarkers[i]);
            _cardClone.Behaviour.Initialize(i, 0);
        }

        // Populate the rival's structure area
        for (int i = 0; i < AvailableDecks[_rand].Structures.Count; i++)
        {
            CardObject _structureCard = AvailableDecks[_rand].Structures[i];

            CardObject _cardClone = new CardObject();
            _cardClone.CardName = _structureCard.CardName;
            _cardClone.CardDescription = _structureCard.CardDescription;
            _cardClone.Sprite = _structureCard.Sprite;
            _cardClone.Type = _structureCard.Type;
            _cardClone.Health = _structureCard.Health;
            _cardClone.MaxHealth = _structureCard.MaxHealth;
            _cardClone.Behaviour = _structureCard.Behaviour;

            RivalStructureLanes[i] = _cardClone;

            // Spawn the card's model on the correct spot
            rivalStructureLaneHologramObjects[i] = Instantiate(_structureCard.Hologram, rivalStructureLaneHologramMarkers[i]);
            _cardClone.Behaviour.Initialize(i, 3);
        }

        onIntro = false;
        coinsAdded = false;

        CameraController.Instance.OnIntro = false;

        if(startingPlayer == 1) SwitchTurn();
    }

    private void HandleWinCondition()
    {
        bool _rivalLost = true;
        bool _playerLost = true;

        // Check for structures on both sides
        for (int i = 0; i < PlayerStructureLanes.Length; i++)
        {
            if(PlayerStructureLanes[i] != null) _playerLost = false;
        }

        for (int i = 0; i < RivalStructureLanes.Length; i++)
        {
            if(RivalStructureLanes[i] != null) _rivalLost = false;
        }

        if(_rivalLost)
        {
            PlayerPriority = true;
            if (!coinsAdded) { DataManager.Instance.addCoins(500);
                if (!victoryAdded) DataManager.Instance.addVictory(1);
            }

            coinsAdded = true;
            
            victoryPanel.SetActive(true);
            DataManager.Instance.totalGames();
        }
        else if(_playerLost)
        {
            PlayerPriority = true;
            if (!coinsAdded) { DataManager.Instance.addCoins(250);
                if (!lostAdded) DataManager.Instance.addLost(1);
            }
            coinsAdded = true;   
            lostAdded = true;
            defeatPanel.SetActive(true);
            DataManager.Instance.totalGames();
        }
    }

/// <summary>
/// Handles switching between the player's and the rival's turn. Calls the OnTurnEffect() event in card behaviours
/// </summary>
    public void SwitchTurn()
    {
        ActionsLeft = actionCapacity;
        RivalActionsLeft = actionCapacity;

        if(PlayerPriority) // The player has priority, switch to rival
        {
            PlayerPriority = false;

            // Switch turn event for rival
            for (int i = 0; i < RivalCreatureLanes.Length; i++)
            {
                if(RivalCreatureLanes[i] != null) 
                {
                    RivalCreatureLanes[i].Behaviour?.OnTurnEffect(i, 2);
                    if(RivalCreatureLanes[i].Poisoned) DamageCard(i, 2, 1);
                }
            }

            for (int i = 0; i < RivalStructureLanes.Length; i++)
            {
                if(RivalStructureLanes[i] != null) RivalStructureLanes[i].Behaviour?.OnTurnEffect(i, 3);
            }

            playerHandTransform.gameObject.SetActive(false);

            isOnAction = true;
            rivalAI.InitializeTurn();
            rivalAI.ProcessAction();
        }
        else // The rival has priority, switch to player
        {
            PlayerPriority = true;

            // Switch turn event for player
            for (int i = 0; i < PlayerCreatureLanes.Length; i++)
            {
                if(PlayerCreatureLanes[i] != null) 
                {
                    PlayerCreatureLanes[i].Behaviour?.OnTurnEffect(i, 1);
                    if(PlayerCreatureLanes[i].Poisoned) DamageCard(i, 1, 1);
                }
            }

            for (int i = 0; i < PlayerStructureLanes.Length; i++)
            {
                if(PlayerStructureLanes[i] != null) 
                {
                    PlayerStructureLanes[i].Behaviour.OnTurnEffect(i, 0);
                }
            }

            playerHandTransform.gameObject.SetActive(true);
        }
    }

    #region Deck Management
/// <summary>
/// Adds a card to a deck
/// </summary>
/// <param name="_player">Set to true if the player is the one taking the action</param>
/// <param name="_card">Card to add into the deck</param>
/// <returns>Returns true if the card was added, returns false if the deck is full</returns>
    public bool AddCardToDeck(bool _player, CardObject _card)
    {
        if(_player && PlayerDeck.Count >= deckCapacity) return false;
        else if(!_player && RivalDeck.Count >= deckCapacity) return false;

        if(_player) PlayerDeck.Add(_card);
        else RivalDeck.Add(_card);

        return true;
    }

/// <summary>
/// Removes a card from a hand
/// </summary>
/// <param name="_player">Set to true if the player is the one taking the action</param>
/// <param name="_card">Card to remove from the deck</param>
/// <returns>Returns true if the card was removed, returns false if the card was not found in the hand</returns>
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

    /// <summary>
/// Draws a card and consumes an action
/// </summary>
/// <param name="_player">Set to true if the player is the one taking the action</param>
/// <returns>Returns true if the card was drawn, returns false if the hand is full</returns>
    public bool DrawCard(bool _player)
    {
        if (_player && PlayerHand.Count <= handCapacity && PlayerDeck.Count > 0)
        {
            // Select a card to draw
            int _rand = Random.Range(0, PlayerDeck.Count - 1);

            // Create the object for the user to interact with
            GameObject _cardObject = Instantiate(cardPrefab, playerHandTransform);
            _cardObject.GetComponentInChildren<CardInstance>().AssignCard(PlayerDeck[_rand]);

            PlayerHand.Add(_cardObject);

            // Remove card from deck
            PlayerDeck.RemoveAt(_rand);

            ActionsLeft--;

            return true;
        }
        else if(RivalHand.Count <= handCapacity && RivalDeck.Count > 0)
        {
            // Select a card to draw
            int _rand = Random.Range(0, RivalDeck.Count - 1);
            RivalHand.Add(RivalDeck[_rand]);

            // Remove card from deck
            RivalDeck.RemoveAt(_rand);

            isOnAction = false;
            RivalActionsLeft--;

            return true;
        }

        // If not enough capacity, return false to notify
        isOnAction = false;
        return false;
    }

/// <summary>
/// Draws a card but does not consume an action
/// </summary>
/// <param name="_player">Set to true if the player is the one taking the action</param>
/// <returns>Returns true if the card was drawn, returns false if the hand is full</returns>
    public bool DrawFreeCard(bool _player)
    {
        if (_player && PlayerHand.Count <= handCapacity && PlayerDeck.Count > 0)
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
        else if(RivalHand.Count <= handCapacity && RivalDeck.Count > 0)
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

    #endregion

    #region Card Play
    public void SelectCardForPlay(CardObject _card, GameObject _cardGameObject)
    {
        if(ActionsLeft <= 0) return;

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

/// <summary>
/// Handles attacks between an attacker card and a defendant card. Calls OnDieEffect() event in card behaviours
/// </summary>
/// <param name="_attackerLane">Lane of attacker card [0-2]</param>
/// <param name="_attackerRow">Row of attacker card, [0-1] for player and [2-3] for rival</param>
/// <param name="_defendantLane">Lane of defendant card [0-2]</param>
/// <param name="_defendantRow">Row of defendant card, [0-1] for player and [2-3] for rival</param>
/// <returns>Returns false if the defendant is a blocked structure, returns true in any other case</returns>
    public bool AttackCard(int _attackerLane, int _attackerRow, int _defendantLane, int _defendantRow)
    {
        // Prevent attacks to structures when there is a creature in the way
        if(_defendantRow == 3 && _attackerRow == 1)
        {
            if(GetCardAt(_defendantLane, 2) != null) return false;
        }
        else if(_defendantRow == 0 && _attackerRow == 2)
        {
            if(GetCardAt(_defendantLane, 1) != null) return false;
        }

        CardObject _attacker = GetCardAt(_attackerLane, _attackerRow);
        CardObject _defendant = GetCardAt(_defendantLane, _defendantRow);

        _defendant.Health -= _attacker.Damage;

        if(_defendant.Protected)
        {
            _defendant.Protected = false;
            return true;
        }

        if(_attackerRow > 1) RivalActionsLeft--;
        else 
        {
            ActionsLeft--;
            rivalAI.AggressionTokens++;
            rivalAI.DefensiveTokens--;
        }

        // Ensure the card has the base death behaviour
        if(_defendant.Behaviour == null) _defendant.Behaviour = new CardBehaviour();

        Debug.Log("Attacked creature: " + _defendant.CardName + " with damage: " + _attacker.Damage + " and new health: " + _defendant.Health);
        
        GameObject _attackerObj = GetCardHologramAt(_attackerLane, _attackerRow);
        GameObject _defendantObj = GetCardHologramAt(_defendantLane, _defendantRow);

        StartCoroutine(AttackTween(_attackerObj, _defendantObj));

        switch(_defendantRow)
        {
            case 0:
                playerStructureLaneHurtEffects[_defendantLane].Play();
                rivalLaneAttackEffects[_attackerLane].Play();
                break;

            case 1:
                playerLaneHurtEffects[_defendantLane].Play();
                rivalLaneAttackEffects[_attackerLane].Play();
                break;

            case 2:
                rivalLaneHurtEffects[_defendantLane].Play();
                playerLaneAttackEffects[_attackerLane].Play();
                break;

            case 3:
                rivalStructureLaneHurtEffects[_defendantLane].Play();
                playerLaneAttackEffects[_attackerLane].Play();
                break;
        }

        if(_defendant.Health <= 0) 
        {
            _defendant.Behaviour.OnDieEffect();

            switch(_defendantRow)
            {
                case 0:
                    Destroy(playerStructureLaneHologramObjects[_defendantLane]);

                    PlayerStructureLanes[_defendantLane] = null;
                    playerStructureLaneHologramObjects[_defendantLane] = null;
                    rivalAI.AggressionTokens++;
                    break;

                case 1:
                    Destroy(playerLaneHologramObjects[_defendantLane]);
                    
                    PlayerCreatureLanes[_defendantLane] = null;
                    playerLaneHologramObjects[_defendantLane] = null;
                    rivalAI.AggressionTokens++;
                    break;

                case 2:
                    Destroy(rivalLaneHologramObjects[_defendantLane]);
                    
                    RivalCreatureLanes[_defendantLane] = null;
                    rivalLaneHologramObjects[_defendantLane] = null;
                    rivalAI.AggressionTokens--;
                    rivalAI.DefensiveTokens++;
                    break;

                case 3:
                    Destroy(rivalStructureLaneHologramObjects[_defendantLane]);
                    
                    RivalStructureLanes[_defendantLane] = null;
                    rivalStructureLaneHologramObjects[_defendantLane] = null;
                    rivalAI.AggressionTokens--;
                    rivalAI.DefensiveTokens++;
                    break;
            }

        }

        return true;
    }

/// <summary>
/// Apply damage to a card. Calls OnDieEffect() event in card behaviours
/// </summary>
/// <param name="_lane">Lane of card to damage [0-2]</param>
/// <param name="_row">Row of card to damage, [0-1] for player and [2-3] for rival</param>
/// <param name="_damage">Damage to apply</param>
    public void DamageCard(int _lane, int _row, int _damage)
    {
        CardObject _defendant = GetCardAt(_lane, _row);

        _defendant.Health -= _damage;

        switch(_row)
        {
            case 0:
                playerStructureLaneHurtEffects[_lane].Play();
                break;

            case 1:
                playerLaneHurtEffects[_lane].Play();
                break;

            case 2:
                rivalLaneHurtEffects[_lane].Play();
                break;

            case 3:
                rivalStructureLaneHurtEffects[_lane].Play();
                break;
        }

        if(_defendant.Health <= 0) 
        {
            _defendant.Behaviour?.OnDieEffect();

            switch(_row)
            {
                case 0:
                    Destroy(playerStructureLaneHologramObjects[_lane]);

                    PlayerStructureLanes[_lane] = null;
                    playerStructureLaneHologramObjects[_lane] = null;
                    rivalAI.AggressionTokens++;
                    break;

                case 1:
                    Destroy(playerLaneHologramObjects[_lane]);
                    
                    PlayerCreatureLanes[_lane] = null;
                    playerLaneHologramObjects[_lane] = null;
                    rivalAI.AggressionTokens++;
                    break;

                case 2:
                    Destroy(rivalLaneHologramObjects[_lane]);
                    
                    RivalCreatureLanes[_lane] = null;
                    rivalLaneHologramObjects[_lane] = null;
                    rivalAI.AggressionTokens--;
                    rivalAI.DefensiveTokens++;
                    break;

                case 3:
                    Destroy(rivalStructureLaneHologramObjects[_lane]);
                    
                    RivalStructureLanes[_lane] = null;
                    rivalStructureLaneHologramObjects[_lane] = null;
                    rivalAI.AggressionTokens--;
                    rivalAI.DefensiveTokens++;
                    break;
            }
        }
    }

    public void ToggleTokenSelectMarker(bool _player, int _lane, int _row, Color _color)
    {
        GameObject _marker;

        if(_player)
        {
            switch(_row)
            {
                case 0:
                    _marker = playerStructureLaneSelectedMarkers[_lane].gameObject;
                    Debug.Log("Marker: " + _marker.name + " Active:" + !_marker.activeInHierarchy);
                    _marker.GetComponent<Renderer>().material.color = _color;
                    _marker.SetActive(!_marker.activeInHierarchy);
                    break;

                case 1:
                    _marker = playerLaneSelectedMarkers[_lane].gameObject;
                    Debug.Log("Marker: " + _marker.name + " Active:" + !_marker.activeInHierarchy);
                    _marker.GetComponent<Renderer>().material.color = _color;
                    _marker.SetActive(!_marker.activeInHierarchy);
                    break;
            }
        }
        else
        {
            switch(_row)
            {
                case 0:
                    _marker = rivalStructureLaneSelectedMarkers[_lane].gameObject;
                    Debug.Log("Marker: " + _marker.name + " Active:" + !_marker.activeInHierarchy);
                    _marker.GetComponent<Renderer>().material.color = _color;
                    _marker.SetActive(!_marker.activeInHierarchy);
                    break;

                case 1:
                    _marker = rivalLaneSelectedMarkers[_lane].gameObject;
                    Debug.Log("Marker: " + _marker.name + " Active:" + !_marker.activeInHierarchy);
                    _marker.GetComponent<Renderer>().material.color = _color;
                    _marker.SetActive(!_marker.activeInHierarchy);
                    break;
            }
        }
    }

/// <summary>
/// Plays selected card in the provided lane. Requires a card to be selected before being called, use SelectCardForPlay(). Calls Initialize() in card behaviours
/// </summary>
/// <param name="_lane">Lane in which to add the card [0-2]</param>
/// <returns>Returns false if no card was selected or if the lane was occupied</returns>
    public bool PlayCard(int _lane)
    {
        if(ActionsLeft <= 0) return false;

        if(selectedCard == null)
        {
            return false;
        }

        cardHolderCards[_lane].gameObject.SetActive(true);
        cardHolderCards[_lane].sprite = selectedCard.Sprite;

        if(selectedCard.Type == CardType.Creature && PlayerCreatureLanes[_lane] == null)
        {
            rivalAI.AggressionTokens--;

            CardObject _cardClone = new CardObject();
            _cardClone.CardName = selectedCard.CardName;
            _cardClone.CardDescription = selectedCard.CardDescription;
            _cardClone.Sprite = selectedCard.Sprite;
            _cardClone.Type = selectedCard.Type;
            _cardClone.Health = selectedCard.Health;
            _cardClone.MaxHealth = selectedCard.MaxHealth;
            _cardClone.Damage = selectedCard.Damage;
            _cardClone.Behaviour = selectedCard.Behaviour;

            PlayerCreatureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            playerLaneHologramObjects[_lane] = Instantiate(selectedCard.Hologram, playerLaneHologramMarkers[_lane]);
            selectedCard.Behaviour?.Initialize(_lane, 1);

            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;

            ActionsLeft--;

            return true;
            
        }
        else if(selectedCard.Type == CardType.Spell)
        {
            rivalAI.AggressionTokens--;

            int _row = 1;

            if(PlayerCreatureLanes[_lane] != null && selectedCard.Beneficial) _row = 1;
            else if(RivalCreatureLanes[_lane] != null) _row = 2;
            else if(RivalStructureLanes[_lane] != null) _row = 3;

            selectedCard.Behaviour.Initialize(_lane, _row);
            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;
            
            ActionsLeft--;

            return true;
        }
        else if(selectedCard.Type == CardType.Structure && PlayerStructureLanes[_lane] == null)
        {
            CardObject _cardClone = new CardObject();
            _cardClone.CardName = selectedCard.CardName;
            _cardClone.CardDescription = selectedCard.CardDescription;
            _cardClone.Sprite = selectedCard.Sprite;
            _cardClone.Type = selectedCard.Type;
            _cardClone.Health = selectedCard.Health;
            _cardClone.MaxHealth = selectedCard.MaxHealth;
            _cardClone.Behaviour = selectedCard.Behaviour;

            PlayerStructureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            playerStructureLaneHologramObjects[_lane] = Instantiate(selectedCard.Hologram, playerStructureLaneHologramMarkers[_lane]);
            selectedCard.Behaviour.Initialize(_lane, 0);
            RemoveCard(true, selectedCard);

            selectedCard = null;
            selectedCardObject = null;
            
            ActionsLeft--;

            return true;
        }
        
        // If the lane is occupied, return false to notify
        return false;
    }

/// <summary>
/// Plays token from the tokenCards[] array in the provided lane, does not consume an action. Calls Initialize() in card behaviours
/// </summary>
/// <param name="_lane">Lane in which to add the token [0-2]</param>
/// <param name="_tokenIndex">Index of the token in the tokenCards[] array</param>
    public void PlayTokenCard(int _lane, int _tokenIndex)
    {
        CardObject _card = tokenCards[_tokenIndex];

        CardObject _cardClone = new CardObject();
        _cardClone.CardName = _card.CardName;
        _cardClone.CardDescription = _card.CardDescription;
        _cardClone.Sprite = _card.Sprite;
        _cardClone.Type = _card.Type;
        _cardClone.Health = _card.Health;
        _cardClone.MaxHealth = _card.MaxHealth;
        _cardClone.Damage = _card.Damage;
        _cardClone.Behaviour = _card.Behaviour;

        PlayerCreatureLanes[_lane] = _cardClone;

        // Spawn the card's model on the correct spot
        playerLaneHologramObjects[_lane] = Instantiate(_card.Hologram, playerLaneHologramMarkers[_lane]);
        _card.Behaviour?.Initialize(_lane, 1);
    }

/// <summary>
/// Plays card from the RivalHand[] list in the provided lane and row. Calls Initialize() in card behaviours.
/// </summary>
/// <param name="_lane">Lane in which to add the card [0-2]</param>
/// <param name="_row">Row in which to add the card, it must be a rival owned row [2-3]</param>
/// <param name="_cardIndex">Index of the card in the RivalHand[] list</param>
/// <returns>Returns false if the lane was occupied</returns>
    public bool PlayRivalCard(int _lane, int _row, int _cardIndex)
    {
        if(RivalActionsLeft <= 0) SwitchTurn();

        CardObject _card = RivalHand[_cardIndex];

        if(_card.Type == CardType.Creature && RivalCreatureLanes[_lane] == null)
        {
            CardObject _cardClone = new CardObject();
            _cardClone.CardName = _card.CardName;
            _cardClone.CardDescription = _card.CardDescription;
            _cardClone.Sprite = _card.Sprite;
            _cardClone.Type = _card.Type;
            _cardClone.Health = _card.Health;
            _cardClone.MaxHealth = _card.MaxHealth;
            _cardClone.Damage = _card.Damage;
            _cardClone.Behaviour = _card.Behaviour;

            RivalCreatureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            rivalLaneHologramObjects[_lane] = Instantiate(_card.Hologram, rivalLaneHologramMarkers[_lane]);
            _card.Behaviour?.Initialize(_lane, 1);

            RemoveCard(false, _card);

            RivalActionsLeft--;
            isOnAction = false;

            return true;
            
        }
        else if(_card.Type == CardType.Spell)
        {
            int _castRow = 2;

            if(RivalCreatureLanes[_lane] != null && _card.Beneficial) _castRow = 2;
            else if(PlayerCreatureLanes[_lane] != null) _castRow = 1;
            else if(PlayerStructureLanes[_lane] != null) _castRow = 0;

            _card.Behaviour.Initialize(_lane, _castRow);
            RemoveCard(false, _card);
            

            RivalActionsLeft--;
            isOnAction = false;

            return true;
        }
        else if(_card.Type == CardType.Structure && RivalStructureLanes[_lane] == null)
        {
            CardObject _cardClone = new CardObject();
            _cardClone.CardName = _card.CardName;
            _cardClone.CardDescription = _card.CardDescription;
            _cardClone.Sprite = _card.Sprite;
            _cardClone.Type = _card.Type;
            _cardClone.Health = _card.Health;
            _cardClone.MaxHealth = _card.MaxHealth;
            _cardClone.Behaviour = _card.Behaviour;

            RivalStructureLanes[_lane] = _cardClone;

            // Spawn the card's model on the correct spot
            rivalStructureLaneHologramObjects[_lane] = Instantiate(_card.Hologram, rivalStructureLaneHologramMarkers[_lane]);
            _card.Behaviour.Initialize(_lane, 0);
            RemoveCard(false, _card);
            
            RivalActionsLeft--;
            isOnAction = false;

            return true;
        }
        
        // If the lane is occupied, return false to notify
        isOnAction = false;
        return false;
    }
    #endregion

/// <summary>
/// Gets a card in the specified position on the board
/// </summary>
/// <param name="_lane">Card's lane [0-2]</param>
/// <param name="_row">Card's row, [0-1] for player and [2-3] for rival</param>
/// <returns>Returns the CardObject in that position, or null if the position is empty</returns>
    public CardObject GetCardAt(int _lane, int _row)
    {
        switch(_row)
        {
            case 0:
                return PlayerStructureLanes[_lane];

            case 1:
                return PlayerCreatureLanes[_lane];

            case 2:
                return RivalCreatureLanes[_lane];

            case 3:
                return RivalStructureLanes[_lane];
        }

        return null;
    }

/// <summary>
/// Gets the gameobject that represents the card in the specified position on the board
/// </summary>
/// <param name="_lane">Card's lane [0-2]</param>
/// <param name="_row">Card's row, [0-1] for player and [2-3] for rival</param>
/// <returns>Returns the GameObject of the card in that position, or null if the position is empty</returns>
    public GameObject GetCardHologramAt(int _lane, int _row)
    {
        switch(_row)
        {
            case 0:
                return playerStructureLaneHologramObjects[_lane];

            case 1:
                return playerLaneHologramObjects[_lane];

            case 2:
                return rivalLaneHologramObjects[_lane];

            case 3:
                return rivalStructureLaneHologramObjects[_lane];
        }

        return null;
    }

    #region Tweens

    private IEnumerator AttackTween(GameObject _attacker, GameObject _defendant)
    {
        InAnimation = true;
        Vector3 _originalAttackerPos = _attacker.transform.position;
        Vector3 _originalAttackerRot = _attacker.transform.rotation.eulerAngles;

        Sequence _attackSequence = DOTween.Sequence();
        _attackSequence.Append(_attacker.transform.DOLookAt(_defendant.transform.position, .2f));
        _attackSequence.Append(_attacker.transform.DOMove(_defendant.transform.position, .7f));
        _attackSequence.AppendCallback(()=>StartCoroutine(DamageTween(_defendant)));    // Lamda required
        _attackSequence.Append(_attacker.transform.DOMove(_originalAttackerPos, .7f));
        _attackSequence.Append(_attacker.transform.DORotate(_originalAttackerRot, .2f));

        _attackSequence.Play();

        yield return _attackSequence.WaitForCompletion();
        isOnAction = false;
        InAnimation = false;
    }

    private IEnumerator DamageTween(GameObject _obj)
    {
        Sequence _damageSequence = DOTween.Sequence();
        _damageSequence.Append(_obj.transform.DOMove(_obj.transform.position - _obj.transform.forward * .1f, .2f));
        _damageSequence.Append(_obj.transform.DOLocalMove(Vector3.zero, .2f));

        _damageSequence.Play();

        yield return _damageSequence.WaitForCompletion();
    }
    
    #endregion
}