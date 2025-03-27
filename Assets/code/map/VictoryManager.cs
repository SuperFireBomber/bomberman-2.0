using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject clearPanel;

    private void Awake()
    {
        instance = this;
    }

    public void ShowVictory()
    {
        clearPanel.SetActive(true);
    }

    public void NextLevel()
    {
        SceneLoader.LoadNextScene();
    }

    public void ReturnToMenu()
    {
        SceneLoader.LoadMenu();
    }
}