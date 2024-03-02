using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRenderer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image background;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text healthText;

    public void Initialize(CardObject _card)
    {
        image.sprite = _card.Sprite;
        //nameText.text = _card.CardName;
        //damageText.text = _card.Damage.ToString();
        //healthText.text = _card.Health.ToString();
    }
}
