using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoMonedas : MonoBehaviour
{
    public DataManager dataManager;
    public TextMeshProUGUI textoMonedas;

    void Update()
    {
        // Verificar si el DataManager est� asignado
        if (dataManager != null)
        {
            // Actualizar el texto con la cantidad de monedas del DataManager
            textoMonedas.text =  dataManager.coins.ToString();
        }
        else
        {
            Debug.LogWarning("El DataManager no est� asignado en el ActualizadorTextoMonedas.");
        }
    }
}
