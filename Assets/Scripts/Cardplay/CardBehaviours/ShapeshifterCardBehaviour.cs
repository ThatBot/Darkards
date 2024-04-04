using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeshifterCardBehaviour : CardBehaviour
{
    bool transformed = false;

    public override void OnCastEffect()
    {
        Shift();

        base.OnCastEffect();
    }

    public override void OnTurnEffect()
    {
        Shift();

        base.OnTurnEffect();
    }

    private void Shift()
    {
        CardObject _shifterObj = CardManager.Instance.GetCardAt(lane, 1);

        if(CardManager.Instance.RivalCreatureLanes[lane] == null)
        {
            _shifterObj.Health = 1;
            _shifterObj.MaxHealth = 1;
            _shifterObj.Damage = 1;

            transformed = false;

            return;
        }

        CardObject _otherObj = CardManager.Instance.GetCardAt(lane, 2);

        _shifterObj.Health = _otherObj.Health;
        _shifterObj.MaxHealth = _otherObj.MaxHealth;
        _shifterObj.Damage = _otherObj.Damage;

        transformed = true;

    }
}
