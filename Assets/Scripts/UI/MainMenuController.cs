using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
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
}
