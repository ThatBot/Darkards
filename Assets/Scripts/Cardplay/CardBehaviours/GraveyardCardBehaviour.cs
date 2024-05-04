using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardCardBehaviour : CardBehaviour
{
    public override void OnTurnEffect(int _lane, int _row)
    {
        // Obtain card depending on which side of the board the structure is at
        CardObject _card = null;
        if (_row == 0) _card = CardManager.Instance.GetCardAt(_lane, 1);
        else if (_row == 3) _card = CardManager.Instance.GetCardAt(_lane, 2);

        if(_card == null)
        {
            Debug.Log("Played card at lane: " + _lane);
            CardManager.Instance.PlayTokenCard(_lane, 0);
        }

        base.OnTurnEffect(_lane, _row);
    }
}
