using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        if(CardManager.Instance.GetCardAt(lane, row) != null) 
            CardManager.Instance.GetCardAt(lane, row).Protected = true;

        if (row == 1) FlaskController.Instance.ChangeColor(new Color(.8f, .6f, 0.0f));

        base.OnCastEffect();
    }
}
