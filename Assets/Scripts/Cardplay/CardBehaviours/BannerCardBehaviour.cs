using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        if(CardManager.Instance.GetCardAt(lane, row) != null) 
            CardManager.Instance.GetCardAt(lane, row).Protected = true;

        base.OnCastEffect();
    }
}
