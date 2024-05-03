using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZuluCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        CardManager.Instance.DamageCard(lane, row, 1);

        if (row <= 2) FlaskController.Instance.ChangeColor(new Color(0.0f, 0.0f, 0.0f));

        base.OnCastEffect();
    }
}
