using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZuluCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Cast death upon ye");

        CardManager.Instance.DamageCard(lane, row, 1);

        base.OnCastEffect();
    }
}
