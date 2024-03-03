using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZuluCardBehaviour : CardBehaviour
{
    public override void OnCastEffect()
    {
        Debug.Log("Cast death upon ye");

        switch(row)
        {
            case 1:
                
                break;

            case 2:

                break;
        }

        base.OnCastEffect();
    }
}
