using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDrawSampleHand : MonoBehaviour
{
    [SerializeField] private List<CardObject> sampleHand = new List<CardObject>();

    public void OnDrawSamplePressed()
    {
        for (int i = 0; i < sampleHand.Count; i++)
        {
            if(CardManager.Instance.AddCardToDeck(true, sampleHand[i])) Debug.Log("Added card " + i + " to deck");
            else Debug.LogWarning("Deck capacity exceded at i=" + i);
        }

        for (int i = 0; i < sampleHand.Count; i++)
        {
            if(!CardManager.Instance.DrawCard(true)) Debug.LogWarning("Hand capacity exceded!");
        }

        CardManager.Instance.AddCardToDeck(false, sampleHand[0]);
        CardManager.Instance.AddCardToDeck(false, sampleHand[2]);

        CardManager.Instance.DrawCard(false);
        CardManager.Instance.DrawCard(false);
        
        CardManager.Instance.PlayRivalCard(1, 2, 0);
        CardManager.Instance.PlayRivalCard(2, 3, 0);
    }
}
