using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoMonedas : MonoBehaviour
{
    public TextMeshProUGUI textoMonedas;

    void Update()
    {
        if (!DataManager.Instance)
        {
            return;
        }    
        textoMonedas.text =  DataManager.Instance.coins.ToString();
    }
}
