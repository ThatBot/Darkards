using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Darkards/Deck")]
public class DeckObject : ScriptableObject
{
    public bool PreselectedStructures;
    public List<CardObject> Structures;
    public List<CardObject> Deck;
}
