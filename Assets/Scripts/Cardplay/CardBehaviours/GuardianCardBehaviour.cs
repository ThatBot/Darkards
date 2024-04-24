using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Casted guardian");

        base.OnCastEffect();
    }

    public override void OnTurnEffect()
    {
        // Obtain card depending on which side of the board the structure is at
        CardObject _card = null;
        if (row == 0) _card = CardManager.Instance.GetCardAt(lane, 1);
        else if (row == 3) _card = CardManager.Instance.GetCardAt(lane, 2);
        if(_card == null) return;

        _card.Health += 1;
        if(_card.Health > _card.MaxHealth) _card.Health = _card.MaxHealth;

        base.OnTurnEffect();
    }
}
