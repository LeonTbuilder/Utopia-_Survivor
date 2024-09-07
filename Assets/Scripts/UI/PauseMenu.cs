using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public string sceneHubName;
    public string nameSceneReset;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadHubScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneHubName);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(nameSceneReset);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void OpenPauseMenu()
    {
        Pause();
        pauseMenuUI.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
