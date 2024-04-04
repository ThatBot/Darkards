using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Darkards/Card")]
public class CardObject : ScriptableObject
{
    [Header("Cosmetic")]
    public string CardName;
    public string CardDescription;
    public Sprite Sprite;
    public GameObject Hologram;

    [Header("Mechanics")]
    public CardType Type;
    public CardBehaviour Behaviour;
    public bool Beneficial;
    public int Cost;
    public int Health;
    public int MaxHealth;
    public int Damage;

    [HideInInspector] public bool Protected;
    [HideInInspector] public bool Poisoned;
}

public enum CardType
{
    Creature = 0,
    Spell,
    Structure
}
