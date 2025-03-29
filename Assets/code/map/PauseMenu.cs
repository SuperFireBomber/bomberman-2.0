using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // 用于显示暂停菜单的Panel
    public GameObject pausePanel;
    // “返回游戏”按钮
    public Button resumeButton;
    // 设置按钮本身
    public Button settingButton;
    // “重新开始”按钮
    public Button restartButton;
    // “返回主菜单”按钮
    public Button menuButton;

    void Start()
    {
        // 初始时隐藏暂停菜单
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        // 为“返回游戏”按钮添加点击事件监听
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        // 为“重新开始”按钮添加点击事件，调用SceneLoader.ReloadGame()
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(SceneLoader.ReloadGame);
        }
        // 为“返回主菜单”按钮添加点击事件，调用SceneLoader.LoadMenu()
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(SceneLoader.LoadMenu);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果面板已经在显示，按 ESC 则恢复游戏
            if (pausePanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                // 面板没显示，就显示它
                OnSettingButtonClick();
            }
        }
    }

    // 点击设置按钮时调用（确保在Inspector中将Setting按钮的OnClick事件绑定此方法）
    public void OnSettingButtonClick()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        // 暂停游戏
        Time.timeScale = 0;
        // 隐藏设置按钮
        if (settingButton != null)
        {
            settingButton.gameObject.SetActive(false);
        }
    }

    // “返回游戏”按钮点击后恢复游戏的方法
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        // 恢复游戏
        Time.timeScale = 1;
        // 显示设置按钮
        if (settingButton != null)
        {
            settingButton.gameObject.SetActive(true);
        }
    }
}
