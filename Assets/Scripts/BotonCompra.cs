using UnityEngine;
using TMPro;

public class BotonCompra : MonoBehaviour
{
    public int costoDelObjeto = 1500;
    public DataManager dataManager;
    public TextMeshProUGUI textoBoton;
    public TextMeshProUGUI textoPropiedad;
    public string propiedadTexto = "Propiedad";
    public GameObject[] botones;
    public GameObject botonSeleccionado;

    private void Start()
    {
        ActualizarEstadoBoton();
    }

    public void ComprarObjeto()
    {
        if (dataManager.coins >= costoDelObjeto)
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
            ActualizarBotonesSeleccionados();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
        ActualizarEstadoBoton(); // Llamar aquí para actualizar el color del precio del botón
    }

    private void ActualizarEstadoBoton()
    {
        if (dataManager.coins >= costoDelObjeto)
        {
            textoBoton.color = Color.green;
        }
        else
        {
            textoBoton.color = Color.red;
        }
    }

    private void ActualizarBotonesSeleccionados()
    {
        foreach (GameObject boton in botones)
        {
            TextMeshProUGUI textoPropiedadBoton = boton.GetComponentInChildren<TextMeshProUGUI>();
            if (textoPropiedadBoton != null)
            {
                if (boton == botonSeleccionado)
                {
                    textoPropiedadBoton.text = "Seleccionado";
                }
                else
                {
                    textoPropiedadBoton.text = dataManager.TieneCarta(boton.name) ? "Propiedad" : "";
                }
            }
        }
    }
}
