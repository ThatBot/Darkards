using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Glossary")]
    [SerializeField] private GameObject descriptionModal;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text loreText;

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
