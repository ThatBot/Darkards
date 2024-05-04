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

    public override void OnTurnEffect(int _lane, int _row)
    {
        // Obtain card depending on which side of the board the structure is at
        CardObject _card = null;
        if (_row == 0) _card = CardManager.Instance.GetCardAt(_lane, 1);
        else if (_row == 3) _card = CardManager.Instance.GetCardAt(_lane, 2);
        if(_card == null) return;

        _card.Health += 1;
        if(_card.Health > _card.MaxHealth) _card.Health = _card.MaxHealth;

        base.OnTurnEffect(_lane, _row);
    }
}
