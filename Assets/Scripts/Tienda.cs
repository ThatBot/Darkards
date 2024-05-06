using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    public BotonCompra[] botones;
    private int select;
    public static Tienda Instance;

    public void Start()
    {
        Instance = this;
        select = PlayerPrefs.GetInt("store.select", 0);
        foreach (var boton in botones)
        {
            if (DataManager.Instance.TieneCarta(boton.name))
            {
                boton.comprado = true;
            }
        }
        ActualizarBotones();
    }
    public void ActualizarBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            BotonCompra boton = botones[i];
            if (boton.comprado) {
                boton.textoPrecio.gameObject.SetActive(false);
                boton.textoPropiedad.gameObject.SetActive(true);
                if (i == select)
                {
                    boton.textoPropiedad.text = "Seleccionado";
                }
                else
                {
                    boton.textoPropiedad.text = "En propiedad";
                }
            } else { 
                boton.textoPropiedad.gameObject.SetActive(false);
                boton.textoPrecio.gameObject.SetActive(true);
                boton.textoPrecio.text = boton.costoDelObjeto.ToString();
                if (DataManager.Instance.coins >= boton.costoDelObjeto)
                {
                    boton.textoPrecio.color = Color.green;
                }
                else
                {
                    boton.textoPrecio.color = Color.red;
                }
            }
        }       
    }
    public void Seleccionar(BotonCompra boton)
    {
        int index = Array.IndexOf(botones, boton);
        select = index;
        PlayerPrefs.SetInt("store.select", select);
        ActualizarBotones();
    }
}
