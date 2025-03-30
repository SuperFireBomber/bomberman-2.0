using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadNextScene()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = false; // Next scene starts normally
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void ReloadGame()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = true; // Prevent victory from triggering during scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void LoadMenu()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = true; // Prevent victory from triggering during scene reload
        SceneManager.LoadScene("start");
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void LoadLevel1()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = false; // Next scene starts normally
        SceneManager.LoadScene("level1");
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void LoadLevel2()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = false; // Next scene starts normally
        SceneManager.LoadScene("level2");
        AudioManager.instance.PlaySFX("button-click");
    }
    public static void LoadLevel3()
    {
        Time.timeScale = 1;  // ensure revover from pause
        EnemyManager.isSceneReloading = false; // Next scene starts normally
        SceneManager.LoadScene("level3");
        AudioManager.instance.PlaySFX("button-click");
    }
}