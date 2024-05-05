using UnityEngine;
using TMPro;

public class BotonCompra : MonoBehaviour
{
    public int costoDelObjeto = 1500;
    public DataManager dataManager;
    public TextMeshProUGUI textoBoton;
    public GameObject objetoParaComprar; // Referencia al objeto que se comprará

    private void Start()
    {
        ActualizarEstadoBoton();
    }

    private void Update()
    {
        ActualizarEstadoBoton();
    }
    public void ComprarObjeto()
    {
        if (dataManager.coins >= costoDelObjeto)
        {
            // Realizar la compra
            dataManager.coins -= costoDelObjeto;
            dataManager.SaveCoins(); // Guardar las monedas actualizadas
            ActualizarEstadoBoton();
            // Desactivar el botón
            gameObject.SetActive(false);
            // Desaparecer el objeto comprado (opcional)
            if (objetoParaComprar != null)
            {
                objetoParaComprar.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar este objeto.");
        }
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
}
