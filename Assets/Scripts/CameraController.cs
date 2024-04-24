using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraController : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private Transform normalMarker;
    [SerializeField] private Transform overheadMarker;
    [SerializeField] private Transform sideMarker;

    [Header("Card UI")]
    [SerializeField] private GameObject cardStatusObj;
    [SerializeField] private Image cardSprite;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private TMP_Text cardHealth;
    [SerializeField] private TMP_Text cardDamage;

    private int selectedTokenLane = -1;
    private int selectedTokenRow = -1;

    private int currentPos = 0; // 0 -> Normal | 1 -> Overhead

    // Update is called once per frame
    void Update()
    {
        if(PauseController.Instance.IsPaused) return;

        if(Input.GetKeyDown(KeyCode.W) && currentPos == 0)
        {
            transform.position = overheadMarker.position;
            transform.rotation = overheadMarker.rotation;
            currentPos = 1;
        }
        else if(Input.GetKeyDown(KeyCode.S) && currentPos == 1)
        {
            transform.position = normalMarker.position;
            transform.rotation = normalMarker.rotation;
            currentPos = 0;
        }
        else if(Input.GetKeyDown(KeyCode.D) && currentPos == 0)
        {
            transform.position = sideMarker.position;
            transform.rotation = sideMarker.rotation;
            currentPos = 2;
        }
        else if(Input.GetKeyDown(KeyCode.A) && currentPos == 2)
        {
            transform.position = normalMarker.position;
            transform.rotation = normalMarker.rotation;
            currentPos = 0;
        }

        Ray _mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_mouseRay, out RaycastHit _hit))
        {
            if(!CardManager.Instance.PlayerPriority) return;    // If it's not the players turn, do not let them play

            if(_hit.transform.CompareTag("CardPlayArea") && Input.GetMouseButtonDown(0))
            {
                int _lane = Int32.Parse(_hit.transform.name);
                CardManager.Instance.PlayCard(_lane - 1);
            }

            if(_hit.transform.CompareTag("CardToken") && _hit.transform.name.Contains("Player"))
            {
                if(selectedTokenLane != -1 && Input.GetMouseButtonDown(0))
                {
                    CardManager.Instance.ToggleTokenSelectMarker(true, selectedTokenLane, selectedTokenRow, Color.white);
                    selectedTokenLane = -1;
                    selectedTokenRow = -1;
                }
                else
                {
                    int _lane = Int32.Parse(_hit.transform.name.Substring(_hit.transform.name.Length - 1)) - 1;

                    int _row = -1;

                    if(_hit.transform.name.Contains("Structure"))
                    {
                        _row = 0;
                    }
                    else if(_hit.transform.name.Contains("Creature"))
                    {
                        _row = 1;
                    }

                    if(CardManager.Instance.GetCardAt(_lane, _row) != null)
                    {
                        CardStatus(_lane, _row);
                        cardStatusObj.SetActive(true);

                        if(Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("Selected token at Lane:" + _lane + " Row:" + _row);

                            CardManager.Instance.GetCardAt(_lane, _row).Behaviour?.OnTokenSelect();
                            CardManager.Instance.ToggleTokenSelectMarker(true, _lane, _row, Color.red);

                            selectedTokenLane = _lane;
                            selectedTokenRow = _row;
                        }
                    }
                }
            }
            else if(_hit.transform.CompareTag("CardToken") && _hit.transform.name.Contains("Rival"))
            {
                int _lane = Int32.Parse(_hit.transform.name.Substring(_hit.transform.name.Length - 1)) - 1;

                int _row = -1;

                if(_hit.transform.name.Contains("Structure"))
                {
                    _row = 3;
                }
                else if(_hit.transform.name.Contains("Creature"))
                {
                    _row = 2;
                }

                if(CardManager.Instance.GetCardAt(_lane, _row) != null)
                {
                    CardStatus(_lane, _row);
                    cardStatusObj.SetActive(true);

                    if(Input.GetMouseButtonDown(0) && selectedTokenLane != -1)
                    {
                        Debug.Log("Selected token at Lane:" + _lane + " Row:" + _row);

                        if(CardManager.Instance.AttackCard(selectedTokenLane, selectedTokenRow, _lane, _row))
                            CardManager.Instance.ActionsLeft--;
                    }
                }
            }
            else if(Input.GetMouseButtonDown(0))
            {
                cardStatusObj.SetActive(false);

                if(selectedTokenLane != -1)
                {
                    CardManager.Instance.ToggleTokenSelectMarker(true, selectedTokenLane, selectedTokenRow, Color.white);
                    selectedTokenLane = -1;
                    selectedTokenRow = -1;
                }
            }
            else
            {
                cardStatusObj.SetActive(false);
            }
            
        }
    }

    private void CardStatus(int _lane, int _row)
    {
        CardObject _card = CardManager.Instance.GetCardAt(_lane, _row);

        if(_card.Poisoned) cardName.color = new Color(0.5f, 1, 0.5f);
        else if (_card.Protected) cardName.color = new Color(1, 0.8f, 0.5f);
        else cardName.color = new Color(1, 1, 1);

        cardSprite.sprite = _card.Sprite;
        cardName.text = _card.CardName;
        cardDescription.text = _card.CardDescription;
        cardHealth.text = _card.Health + "/" + _card.MaxHealth;
        cardDamage.text = _card.Damage.ToString();
    }
}
