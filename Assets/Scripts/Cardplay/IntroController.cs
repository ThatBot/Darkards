using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    #region Singleton

    public static IntroController Instance;

    void Awake()
    {
        Instance = this;
    }

    #endregion

    [Header("Intro Sequence")]
    [SerializeField] private Animator coinAnimator;
    [SerializeField] private MeshRenderer coinRenderer;
    [SerializeField] private Material coinHeadsMaterial;
    [SerializeField] private Material coinTailsMaterial;
    [SerializeField] private GameObject structureSelector;

    public void InitiateIntro()
    {
        float _coinFlip = Random.Range(0, 100);
        if(_coinFlip >= 50) 
        {
            Debug.Log("Heads");
            List<Material> _mats = new List<Material>();
            _mats.Add(coinTailsMaterial);
            _mats.Add(coinHeadsMaterial);
            coinRenderer.SetMaterials(_mats);
        }
        else
        {
            Debug.Log("Tails");
            List<Material> _mats = new List<Material>();
            _mats.Add(coinHeadsMaterial);
            _mats.Add(coinTailsMaterial);
            coinRenderer.SetMaterials(_mats);
        }
        coinAnimator.Play("Take 001");
    }

    public void SelectStructure()
    {
        structureSelector.SetActive(true);
    }
}
