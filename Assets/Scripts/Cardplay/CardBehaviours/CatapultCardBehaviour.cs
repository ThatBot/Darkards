using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultCardBehaviour : CardBehaviour
{
    public override void OnTurnEffect(int _lane, int _row)
    {
        // Damage card depending on which side of the board the structure is at
        if(CardManager.Instance.GetCardAt(_lane,2) != null)
        {
            CardManager.Instance.DamageCard(_lane, 2, 1);
        }
        else if(CardManager.Instance.GetCardAt(_lane,3) != null)
        {
            CardManager.Instance.DamageCard(_lane, 3, 1);
        }

        base.OnTurnEffect(_lane, _row);
    }
}
