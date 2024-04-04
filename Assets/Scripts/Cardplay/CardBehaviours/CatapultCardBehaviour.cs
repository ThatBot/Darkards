using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultCardBehaviour : CardBehaviour
{
    public override void OnTurnEffect()
    {
        if(CardManager.Instance.GetCardAt(lane,2) != null)
        {
            CardManager.Instance.DamageCard(lane, 2, 1);
        }
        else if(CardManager.Instance.GetCardAt(lane,3) != null)
        {
            CardManager.Instance.DamageCard(lane, 3, 1);
        }

        base.OnTurnEffect();
    }
}
