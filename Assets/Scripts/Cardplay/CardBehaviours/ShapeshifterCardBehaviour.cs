using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeshifterCardBehaviour : CardBehaviour
{
    bool transformed = false;

    public override void OnCastEffect()
    {
        Shift(lane, row);

        base.OnCastEffect();
    }

    public override void OnTurnEffect(int _lane, int _row)
    {
        Shift(_lane, _row);

        base.OnTurnEffect(_lane, _row);
    }

    private void Shift(int _lane, int _row)
    {
        CardObject _shifterObj = CardManager.Instance.GetCardAt(_lane, 1);

        if(CardManager.Instance.RivalCreatureLanes[_lane] == null)
        {
            _shifterObj.Health = 1;
            _shifterObj.MaxHealth = 1;
            _shifterObj.Damage = 1;

            transformed = false;

            return;
        }

        CardObject _otherObj = CardManager.Instance.GetCardAt(_lane, 2);

        _shifterObj.Health = _otherObj.Health;
        _shifterObj.MaxHealth = _otherObj.MaxHealth;
        _shifterObj.Damage = _otherObj.Damage;

        transformed = true;

    }
}
