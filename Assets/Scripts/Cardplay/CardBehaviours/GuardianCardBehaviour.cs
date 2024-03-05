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
        CardObject _card = CardManager.Instance.GetCardAt(lane, 1);
        _card.Health += 1;
        if(_card.Health > _card.MaxHealth) _card.Health = _card.MaxHealth;

        base.OnTurnEffect();
    }
}
