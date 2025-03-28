using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public GameObject clearPanel;
    public GameObject confirmPanel;
    public bool allowMove = true;
    private void Awake()
    {
        instance = this;
    }

    public void ShowVictory()
    {
        clearPanel.SetActive(true);
        AudioManager.instance.PlaySFX("victory");
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
        allowMove = false;
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