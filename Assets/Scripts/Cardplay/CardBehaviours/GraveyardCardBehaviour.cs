using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardCardBehaviour : CardBehaviour
{
    public override void OnTurnEffect()
    {
        // Obtain card depending on which side of the board the structure is at
        CardObject _card = null;
        if (row == 0) _card = CardManager.Instance.GetCardAt(lane, 1);
        else if (row == 3) _card = CardManager.Instance.GetCardAt(lane, 2);

        if(_card == null)
        {
            CardManager.Instance.PlayTokenCard(lane, 0);
        }

        base.OnTurnEffect();
    }
}
