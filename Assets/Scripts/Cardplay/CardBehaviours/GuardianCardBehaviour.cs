using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Casted guardian");

        base.OnCastEffect();
    }
}
