using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    private int lane;
    private int row;

    public virtual void Initialize(int _lane, int _row)
    {
        lane = _lane;
        row = _row;

        OnCastEffect();
    }

    public virtual void OnCastEffect()
    {

    }

    public virtual void OnTurnEffect()
    {

    }

    public virtual void OnDieEffect()
    {

    }
}