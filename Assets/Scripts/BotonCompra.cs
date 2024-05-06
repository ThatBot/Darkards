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
    private bool elegido = false;

    private void Start()
    {
        ActualizarBotones();
        textoPrecio.gameObject.SetActive(true);
        textoPropiedad.gameObject.SetActive(false);
    }

    private void Update()
    {
        ActualizarBotones();
    }

    public void ComprarObjeto()
    {
        if (dataManager.coins >= costoDelObjeto)
        {
            // Realizar la compra
            dataManager.coins -= costoDelObjeto;
            dataManager.GuardarMonedas(); // Guardar las monedas actualizadas

            // Marcar esta carta como en propiedad
            textoPrecio.text = "";
            textoPropiedad.text = "En propiedad";
            dataManager.AñadirCarta(gameObject.name);

            // Actualizar los botones seleccionados
            botonSeleccionado = gameObject;
            ActualizarBotones();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
    }

    public void SeleccionarBoton()
    {
        // Cambiar el estado de los botones
        foreach (GameObject boton in botones)
        {
            if (boton == gameObject)
            {
                boton.GetComponentInChildren<TextMeshProUGUI>().text = "Seleccionado";
            }
            else
            {
                boton.GetComponentInChildren<TextMeshProUGUI>().text = "En propiedad";
                BotonCompra botonCompra = boton.GetComponent<BotonCompra>();
                if (botonCompra != null)
                {
                    botonCompra.ActualizarPropiedad(); // Actualizar el estado "En propiedad"
                }
            }
        }

        // Actualizar el botón seleccionado
        botonSeleccionado = gameObject;
    }

    private void ActualizarBotones()
    {
        foreach (GameObject boton in botones)
        {
            if (boton != botonSeleccionado)
            {
                BotonCompra botonCompra = boton.GetComponent<BotonCompra>();
                if (botonCompra != null)
                {
                    botonCompra.ActualizarPropiedad(); // Actualizar el estado "En propiedad"
                }
            }
        }
    }

    private void ActualizarPropiedad()
    {
        // Actualizar el estado "En propiedad"
        textoPrecio.text = (botonSeleccionado == gameObject) ? costoDelObjeto.ToString() : "";
        textoPropiedad.text = dataManager.TieneCarta(gameObject.name) ? "En propiedad" : "";
    }
}
