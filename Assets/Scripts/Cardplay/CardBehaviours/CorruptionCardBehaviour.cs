using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        if(CardManager.Instance.GetCardAt(lane, row) != null) 
            CardManager.Instance.GetCardAt(lane, row).Poisoned = true;

        if (row <= 2) FlaskController.Instance.ChangeColor(new Color(0.0f, 1.0f, 0.2f));

        base.OnCastEffect();
    }
}
