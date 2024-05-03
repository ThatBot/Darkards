using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlaskController : MonoBehaviour
{
    #region Singleton Definition
    public static FlaskController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
	
    [SerializeField] private Material material;
	
    void Start()
    {
        material.color = Color.blue;
    }

    public void ChangeColor(Color _color)
    {
        material.DOColor(_color, 0.3f);
        //cambioColor(new Color(0, 0, 0));
    }
}
