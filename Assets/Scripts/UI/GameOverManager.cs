using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public string nextHubName;
    public string sceneResetName;
    public string lastHubScene;

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void WinGame()
    {
        gameWinUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneResetName);
    }

    public void GoToNextHubScene()
    {
        SceneManager.LoadScene(nextHubName);
    }

    public void GotoLastHubScene()
    {
        SceneManager.LoadScene(lastHubScene);
    }

    public void BossHubScene()
    {
        SceneManager.LoadScene("FinalBossLvl");
    }
}
