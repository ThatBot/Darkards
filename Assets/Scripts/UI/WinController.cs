using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public void Rematch()
    {
        SceneManager.LoadScene("sc_prototype");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("sc_mainmenu");
    }
}
