using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Darkcards/Card")]
public class CardObject : ScriptableObject
{
    [Header("Cosmetic")]
    public string CardName;
    public Sprite Sprite;
    public GameObject Hologram;

    [Header("Mechanics")]
    public CardType Type;
    public CardBehaviour Behaviour;
    public int Cost;
    public int Health;
    public int Damage;
}

public enum CardType
{
    Creature = 0,
    Spell,
    Structure
}
