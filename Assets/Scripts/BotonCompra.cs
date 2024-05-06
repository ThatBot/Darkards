using UnityEngine;
using TMPro;

public class BotonCompra : MonoBehaviour
{
    public int costoDelObjeto = 1500;
    public DataManager dataManager;
    public TextMeshProUGUI textoPrecio;
    public TextMeshProUGUI textoPropiedad;
    public GameObject[] botones;
    public GameObject botonSeleccionado;
    public bool comprado = false;

    private void Start()
    {
        ActualizarBotones();
    }

    private void Update()
    {
        ActualizarBotones();
    }

    public void ComprarObjeto()
    {
        if (comprado)
        {
            
        }else if (dataManager.coins >= costoDelObjeto)
        {
            // Realizar la compra
            dataManager.coins -= costoDelObjeto;
            dataManager.GuardarMonedas(); // Guardar las monedas actualizadas
            // Marcar esta carta como en propiedad y seleccionada
            if (!dataManager.TieneCarta(gameObject.name))
            {
                textoPropiedad.text = "Propiedad";
                dataManager.AñadirCarta(gameObject.name);
            }
            else
            {
                textoPropiedad.text = "";
            }
            botonSeleccionado = gameObject;
            ActualizarBotones();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
        

    }

    private void ActualizarBotones()
    {
        foreach (GameObject boton in botones)
        {
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
