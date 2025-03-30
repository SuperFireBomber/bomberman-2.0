using UnityEngine;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject clearPanel;
    public GameObject confirmPanel;
    private void Awake()
    {
        instance = this;
    }

    public void ShowVictory()
    {
        if (PlayerHealthController.instance != null && PlayerHealthController.instance.gameObject.activeSelf)
        {
            Time.timeScale = 0;
            AudioManager.instance.PlaySFX("victory");
            clearPanel.SetActive(true);
        }
            
    }
    public void NextLevel()
    {
        SceneLoader.LoadNextScene();
    }
    public void ReturnToMenu()
    {
        SceneLoader.LoadMenu();
    }

    public void ReturnAttempt()
    {
        ConfirmReturn();
    }

    public void ConfirmReturn()
    {
        clearPanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void UndoReturn()
    {
        confirmPanel.SetActive(false);
        clearPanel.SetActive(true);
    }
}