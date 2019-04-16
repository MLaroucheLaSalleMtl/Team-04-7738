using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager code;

    void Start()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        code = FindObjectOfType<GameManager>();
    }

    public void PlayGame()
    {
        code.SetLevelComplete();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
