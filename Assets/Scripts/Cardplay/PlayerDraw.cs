using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDraw : MonoBehaviour
{
    public void OnDrawPressed()
    {
        CardManager.Instance.DrawCard(true);
    }
}
