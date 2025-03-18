using UnityEngine;
using UnityEngine.SceneManagement;


//记得删除。跳转进入关卡选择
public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); 
    }
}
