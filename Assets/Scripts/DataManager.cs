using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int colorblindnessType = 0;
    public int coins = 0;
    private int victorias = 0;
    private int derrotas = 0;
    private int partidas = 0;

    [SerializeField] private Slider master;
    [SerializeField] private Slider musica;
    [SerializeField] private Slider efectos;

    [SerializeField] private float defaultMasterVolume = 1f;
    [SerializeField] private float defaultMusicVolume = 1f;
    [SerializeField] private float defaultEffectsVolume = 1f;
    [SerializeField] private int defaultVictorias = 0;
    [SerializeField] private int defaultDerrotas = 0;
    [SerializeField] private int defaultPartidas = 0;

    // Start is called before the first frame update

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadSettings();
        totalGames();
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

        if (PlayerPrefs.HasKey("Colorblindness"))
        {
            colorblindnessType = PlayerPrefs.GetInt("Colorblindness");
        }
        else
        {
            PlayerPrefs.SetFloat("Colorblindness", 3);
        }

        if (PlayerPrefs.HasKey("Victorias"))
        {
            victorias = PlayerPrefs.GetInt("Victorias");
        }
        else
        {
            victorias = defaultVictorias;
            PlayerPrefs.SetInt("Victorias", defaultVictorias);
        }
        if (PlayerPrefs.HasKey("Derrotas"))
        {
            derrotas = PlayerPrefs.GetInt("Derrotas");
        }
        else
        {
            derrotas = defaultDerrotas;
            PlayerPrefs.SetInt("Derrotas", defaultDerrotas);
        }
        if (PlayerPrefs.HasKey("Partidas"))
        {
            partidas = PlayerPrefs.GetInt("Partidas");
        }
        else
        {
            partidas = defaultPartidas;
            PlayerPrefs.SetInt("Partidas", defaultPartidas);
        }
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }
        else
        {
            coins = 0;
            PlayerPrefs.SetInt("coins", 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
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
    
    public void SetColorblindnessType(int _type)
    {
        colorblindnessType = _type;
        PlayerPrefs.SetInt("Colorblindness", colorblindnessType);
    }

    public void ResetDefaultValues()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();
    }

    public void addCoins(int monedas)
    {
        coins += monedas;
        PlayerPrefs.SetInt("coins", coins);
    }

    public void addVictory(int victory)
    {
        victorias += victory;
        PlayerPrefs.SetInt("Victorias", victorias);
    }

    public void addLost(int lost)
    {
        derrotas += lost;
        PlayerPrefs.SetInt("Derrotas", derrotas);
    }

    public void totalGames()
    {
        partidas = victorias + derrotas;
        PlayerPrefs.SetInt("PartidasTotales", partidas);
    }
    public void SaveCoins()
    {
        PlayerPrefs.SetInt("coins", coins);
    }
}
