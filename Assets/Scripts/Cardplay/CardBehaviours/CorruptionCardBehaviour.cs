using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        if(CardManager.Instance.GetCardAt(lane, row) != null) CardManager.Instance.GetCardAt(lane, row).Poisoned = true;

        base.OnCastEffect();
    }
}
