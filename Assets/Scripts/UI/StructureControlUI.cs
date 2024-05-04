using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StructureControlUI : MonoBehaviour
{
    [SerializeField] private DeckObject playerDeck;
    [SerializeField] private CardObject[] availableStructures;
    [SerializeField] private RectTransform backTransform;
    [SerializeField] private Button[] laneButtons;
    
    private int selectedIndex = -1;

    public void OnEnable()
    {
        backTransform.DOLocalMove(Vector3.zero, 0.5f);
        for (int i = 0; i < playerDeck.Structures.Count; i++)
        {
            laneButtons[i].image.sprite = playerDeck.Structures[i].Sprite;
        }
    }

    public void SelectCard(int _card)
    {
        selectedIndex = _card;
    }

    public void SelectLane(int _lane)
    {
        if(selectedIndex != -1)
        {
            playerDeck.Structures[_lane] = availableStructures[selectedIndex];
            laneButtons[_lane].image.sprite = availableStructures[selectedIndex].Sprite;
        }
    }

    public void OnDone()
    {
        CardManager.Instance.PopulateField();
        gameObject.SetActive(false);
    }
}
