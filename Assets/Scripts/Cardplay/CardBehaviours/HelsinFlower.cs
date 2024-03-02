using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelsinFlower : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Healed card");

        base.OnCastEffect();
    }
}
