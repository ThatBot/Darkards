using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public static PauseController Instance;
    public bool IsPaused;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }

    public void OnContinue()
    {
        pauseMenu.SetActive(false);
        IsPaused = false;
    }

    public void OnToMainMenu()
    {
        SceneManager.LoadScene("sc_mainmenu");
    }
}
