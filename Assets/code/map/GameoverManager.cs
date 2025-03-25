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
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        // 加载名为 "start" 的场景
        SceneManager.LoadScene("start");
    }
}
