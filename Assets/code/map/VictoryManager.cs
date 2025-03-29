using UnityEngine;
using System.Collections;

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
        allowMove = false;
        StartCoroutine(ShowVictoryPanelAfterDelay());
    }

    private IEnumerator ShowVictoryPanelAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        // 检查玩家是否还存在且处于活动状态
        if (PlayerHealthController.instance != null && PlayerHealthController.instance.gameObject.activeSelf)
        {
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