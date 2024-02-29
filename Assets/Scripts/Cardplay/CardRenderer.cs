using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRenderer : MonoBehaviour
{
    public CardObject Card;

    [Header("References")]
    [SerializeField] private Image background;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text healthText;

    void Start()
    {
        image.sprite = Card.Sprite;
        nameText.text = Card.CardName;
        damageText.text = Card.Damage.ToString();
        healthText.text = Card.Health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
