using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    public BotonCompra[] botones;
    private int select;

    public void Start()
    {
        select = PlayerPrefs.GetInt("store.select", 0);
    }
    private void ActualizarBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            BotonCompra boton = botones[i];
            if (boton.comprado) {
                boton.textoPrecio = " ";
                if (i == select)
                {
                    boton.textoPropiedad.text = "Seleccionado";
                }
                else
                {
                    boton.textoPropiedad.text = "En propiedad";
                }
            } else { 
            
            }
        }
        
            TextMeshProUGUI textoPrecioBoton = boton.GetComponentInChildren<TextMeshProUGUI>();
            if (textoPrecioBoton != null)
            {
                if (boton == botonSeleccionado)
                {
                    textoPrecioBoton.color = Color.green;
                    textoPrecioBoton.text = ""; // Ocultar el precio del botón seleccionado
                }
                else
                {
                    BotonCompra botonCompra = boton.GetComponent<BotonCompra>(); // Obtener el script BotonCompra del botón
                    if (botonCompra != null)
                    {
                        textoPrecioBoton.color = (dataManager.coins >= botonCompra.costoDelObjeto) ? Color.green : Color.red; // Actualizar el color del precio
                        textoPrecioBoton.text = botonCompra.costoDelObjeto.ToString(); // Mostrar el precio del botón no seleccionado
                    }
                }
            }
            
        }
    }

}
