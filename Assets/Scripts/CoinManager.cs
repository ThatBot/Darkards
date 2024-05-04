using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coins = 100;
    public TextMeshProUGUI coinsText;
    private string coinsKey = "Coins";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadCoins();
        UpdateCoinsText();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinsText();
        SaveCoins();
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;
        UpdateCoinsText();
        SaveCoins();
    }

    void UpdateCoinsText()
    {
        coinsText.text = " " + coins.ToString();
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt(coinsKey, coins);
        PlayerPrefs.Save();
    }

    void LoadCoins()
    {
        if (PlayerPrefs.HasKey(coinsKey))
        {
            coins = PlayerPrefs.GetInt(coinsKey);
        }
    }
}

public class GameResultHandler : MonoBehaviour
{
    public bool _playerLost;
    public bool _rivalLost;

    public void HandleWinCondition()
    {
        if (_rivalLost)
        {
            CoinManager.instance.AddCoins(500); // Ganas 500 monedas si el rival pierde
        }
        else if (_playerLost)
        {
            CoinManager.instance.AddCoins(250); // Ganas 250 monedas si pierdes
        }
    }
}
