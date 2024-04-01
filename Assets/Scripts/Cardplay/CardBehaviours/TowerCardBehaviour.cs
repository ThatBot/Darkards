using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCardBehaviour : CardBehaviour
{
    private List<CardObject> affectedCards = new List<CardObject>();

    public override void OnCastEffect()
    {
        Debug.Log("Casted tower");

        base.OnCastEffect();
    }

    public override void OnTurnEffect()
    {
        CardObject _card = CardManager.Instance.GetCardAt(lane, 1);
        if(_card == null) return;
        
        if(!affectedCards.Contains(_card))
        {
            _card.Damage += 2;
            affectedCards.Add(_card);
        }

        base.OnTurnEffect();
    }
}
