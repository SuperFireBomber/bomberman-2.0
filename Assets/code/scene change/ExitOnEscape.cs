using UnityEditor;
using UnityEngine;

public class ExitOnEscape : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
        #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
        }
    }
}
