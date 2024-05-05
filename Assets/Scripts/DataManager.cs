using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DataManager : MonoBehaviour
{
    #region Singleton Definition
    public static DataManager Instance;

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

    private int coins = 100;
    [SerializeField] private Slider master;
    [SerializeField] private Slider musica;
    [SerializeField] private Slider efectos;

    [SerializeField] private float defaultMasterVolume = 1f;
    [SerializeField] private float defaultMusicVolume = 1f;
    [SerializeField] private float defaultEffectsVolume = 1f;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }
    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            master.value = PlayerPrefs.GetFloat("Master");
        }
        else
        {
            master.value = defaultMasterVolume;
            PlayerPrefs.SetFloat("Master", defaultMasterVolume);
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            musica.value = PlayerPrefs.GetFloat("Musica");
        }
        else
        {
            musica.value = defaultMusicVolume;
            PlayerPrefs.SetFloat("Music", defaultMusicVolume);
        }

        if (PlayerPrefs.HasKey("Efectos"))
        {
            efectos.value = PlayerPrefs.GetFloat("Efectos");
        }
        else
        {
            efectos.value = defaultEffectsVolume;
            PlayerPrefs.SetFloat("Effects", defaultEffectsVolume);
        }  
    }

    public void SetMasterVolume()
    {
        PlayerPrefs.SetFloat("Master", master.value);
    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("Musica", musica.value);
    }
    public void SetEffectsVolume()
    {
        PlayerPrefs.SetFloat("Efectos", efectos.value);
    }
    
    public void ResetDefaultValues()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();
    }

    public void addCoins(int monedas)
    {
        coins += monedas;
        PlayerPrefs.SetFloat("coins", coins);
    }
}
