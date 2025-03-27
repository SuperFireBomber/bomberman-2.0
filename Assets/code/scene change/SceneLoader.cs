using UnityEngine;
using UnityEngine.SceneManagement;


//记得删除。跳转进入关卡选择
public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void LoadMenu()
    {
        // 加载名为 "start" 的场景
        SceneManager.LoadScene("start");
    }
}