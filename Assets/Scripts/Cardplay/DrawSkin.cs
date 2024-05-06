using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawSkin : MonoBehaviour
{
    public Sprite [] sprites;
    public Image mazo;
    // Start is called before the first frame update
    void Start()
    {
        int index = PlayerPrefs.GetInt("store.select", 0);
        mazo.sprite = sprites[index];
    }

}
