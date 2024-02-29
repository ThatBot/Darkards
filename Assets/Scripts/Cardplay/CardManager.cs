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
    public List<CardObject> PlayerHand = new List<CardObject>();
    public List<CardObject> PlayerDeck = new List<CardObject>();
    public CardObject[] PlayerLanes = new CardObject[3];

    [Header("Rival Data")]
    public List<CardObject> RivalHand = new List<CardObject>();
    public List<CardObject> RivalDeck = new List<CardObject>();
    public CardObject[] RivalLanes = new CardObject[3];

    public bool DrawCard(bool _player)
    {
        if (_player && PlayerHand.Count <= handCapacity)
        {
            int rand = Random.Range(0, PlayerDeck.Count - 1);
            PlayerHand.Add(PlayerDeck[rand]);
            PlayerDeck.RemoveAt(rand);

            return true;
        }
        else if(RivalHand.Count <= handCapacity)
        {
            int rand = Random.Range(0, RivalDeck.Count - 1);
            RivalHand.Add(RivalDeck[rand]);
            RivalDeck.RemoveAt(rand);

            return true;
        }

        // If not enough capacity, return false to notify
        return false;
    }

    public bool PlayCard(CardObject _card, int _lane)
    {
        if(PlayerLanes[_lane] == null)
        {
            PlayerLanes[_lane] = _card;
            PlayerHand.Remove(_card);
            return true;
        }

        // If the lane is occupied, return false to notify
        return false;
    }
}
