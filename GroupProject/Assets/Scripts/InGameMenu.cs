using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    bool Pressed = false;
    public GameObject Menu;

    void Update()
    {
        if (Pressed == false && Input.GetAxisRaw("Cancel") > 0)
        {
            Pressed = true;
            if (Menu.activeInHierarchy == false)
            {
                SetOn();
            }
            else
            {
                SetOff();
            }

        }
        else if (Pressed == true && Input.GetAxisRaw("Cancel") == 0)
        {
            Pressed = false;
        }
    }
    public void SetOn()
    {
        Time.timeScale = 0;
        Menu.SetActive(true);
    }

    public void SetOff()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
