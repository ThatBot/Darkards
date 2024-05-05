using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    [Header("Glossary")]
    [SerializeField] private GameObject descriptionModal;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text loreText;

    [Header("Store")]
    [SerializeField] private TMP_Text coinText;

    public void Start()
    {
        // Populate the sliders with the appropiate values
        float _vol;
        mainMixer.GetFloat("master_vol", out _vol);
        masterSlider.value = _vol;

        mainMixer.GetFloat("music_vol", out _vol);
        musicSlider.value = _vol;

        mainMixer.GetFloat("effects_vol", out _vol);
        effectsSlider.value = _vol;

        coinText.text = PlayerPrefs.GetFloat("coins").ToString();
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("sc_prototype");
    }

    public void OnSettingsPressed()
    {

    }

    public void OnExitPressed()
    {
        Application.Quit();
    }

    public void OnSettingsBackPressed()
    {
        
    }

    public void SetMainVolume(float _vol)
    {
        mainMixer.SetFloat("master_vol", _vol);
    }

    public void SetMusicVolume(float _vol)
    {
        mainMixer.SetFloat("music_vol", _vol);
    }

    public void SetEffectsVolume(float _vol)
    {
        mainMixer.SetFloat("sfx_vol", _vol);
    }

    public void SetCardLore(CardObject _card)
    {
        damageText.text = _card.Damage.ToString();
        healthText.text = _card.Health.ToString();

        loreText.text = _card.CardLore;
        descriptionText.text = _card.CardDescription;
        nameText.text = _card.CardName;

        descriptionModal.SetActive(true);
    }

    public void ClearCardLore()
    {
        descriptionModal.SetActive(false);
    }
}
