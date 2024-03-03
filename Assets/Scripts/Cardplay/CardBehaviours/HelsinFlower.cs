using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelsinFlower : CardBehaviour
{
    public override void OnCastEffect()
    {
        CardObject _card = CardManager.Instance.GetCardAt(lane, row);

        Debug.Log("Healed card: " + _card.CardName);
        Debug.Log("Old card health: " + _card.Health);

        _card.Health += 2;
        if(_card.Health > _card.MaxHealth) _card.Health = _card.MaxHealth;

        Debug.Log("New card health: " + _card.Health);

        base.OnCastEffect();
    }
}
