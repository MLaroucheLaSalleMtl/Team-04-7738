using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager code;
    private EventSystem myEventSystem;

    void Start()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        code = FindObjectOfType<GameManager>();
        myEventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
        StartCoroutine("HighlightBtn");
    }

    IEnumerator HighlightBtn()//code taken from https://answers.unity.com/questions/1011523/first-selected-gameobject-not-highlighted.html sets first button as highlighted in pause menu
    {
        myEventSystem.SetSelectedGameObject(null);
        yield return null;
        myEventSystem.SetSelectedGameObject(myEventSystem.firstSelectedGameObject);
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
