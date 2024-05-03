using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureControlUI : MonoBehaviour
{
    [SerializeField] private DeckObject playerDeck;
    

    private int selectedIndex = -1;

    public void SelectCard(int _card)
    {
        selectedIndex = _card;

    }
}
