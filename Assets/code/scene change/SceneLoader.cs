    using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void LoadMenu()
    {
        SceneManager.LoadScene("start");
        AudioManager.instance.PlaySFX("button-click");
    }
}