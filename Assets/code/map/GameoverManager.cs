using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject gameOverPanel;

    private void Awake()
    {
        instance = this;
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);

    }

    public void Retry()
    {
        SceneLoader.ReloadGame();
    }

    public void ReturnToMenu()
    {
        SceneLoader.LoadMenu();
    }
}
