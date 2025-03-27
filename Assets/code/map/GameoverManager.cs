using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
