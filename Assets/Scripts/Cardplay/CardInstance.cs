using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    [SerializeField] private float pointerOffset = 50.0f;
    public CardObject Card;
    private Outline outline;
    private bool selected;

    public void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void AssignCard(CardObject _card)
    {
        Card = _card;
        gameObject.GetComponent<CardRenderer>().Initialize(_card);
    }

    public void OnPointerEnter()
    {
        transform.localPosition = Vector3.up * pointerOffset;
    }

    public void OnPointerExit()
    {
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerClick()
    {
        if(selected) DeselectCard();
        else SelectCard();
    }

    public void SelectCard()
    {
        outline.enabled = true;
        selected = true;
        CardManager.Instance.SelectCardForPlay(Card, gameObject);
    }

    public void DeselectCard()
    {
        outline.enabled = false;
        selected = false;
        CardManager.Instance.DeselectCardForPlay();
    }
}
