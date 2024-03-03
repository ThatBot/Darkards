using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Casted tower");

        base.OnCastEffect();
    }
}
