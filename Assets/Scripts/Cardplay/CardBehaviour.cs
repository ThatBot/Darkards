using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    protected int lane;
    protected int row;

/// <summary>
/// Set the card's parameters to be used in later events
/// </summary>
/// <param name="_lane">Card's lane [0-2]</param>
/// <param name="_row">Card's row [0-1] for player & [2-3] for rival</param>
    public virtual void Initialize(int _lane, int _row)
    {
        lane = _lane;
        row = _row;
        
        OnCastEffect();
    }

/// <summary>
/// Event triggered when card is played
/// </summary>
    public virtual void OnCastEffect()
    {

    }

/// <summary>
/// Event triggered when the card's token is selected
/// </summary>
    public virtual void OnTokenSelect()
    {
        
    }

/// <summary>
/// Event triggered when turns switch (priority rotates)
/// </summary>
    public virtual void OnTurnEffect(int _lane, int _row)
    {
        
    }

/// <summary>
/// Event triggered when the card dies
/// </summary>
    public virtual void OnDieEffect()
    {
        
    }
}
