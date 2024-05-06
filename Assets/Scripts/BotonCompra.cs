using UnityEngine;
using TMPro;

public class BotonCompra : MonoBehaviour
{
    public int costoDelObjeto = 1500;
    public TextMeshProUGUI textoPrecio;
    public TextMeshProUGUI textoPropiedad;
    public bool comprado = false;

    public void ComprarObjeto()
    {
        if (comprado)
        {
            Tienda.Instance.Seleccionar(this);
        }
        else if (DataManager.Instance.coins >= costoDelObjeto)
        {
            // Realizar la compra
            DataManager.Instance.coins -= costoDelObjeto;
            DataManager.Instance.SaveCoins(); // Guardar las monedas actualizadas
            // Marcar esta carta como en propiedad y seleccionada
            DataManager.Instance.AñadirCarta(name);
            comprado = true;
            Tienda.Instance.Seleccionar(this);
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
        

    }
}
