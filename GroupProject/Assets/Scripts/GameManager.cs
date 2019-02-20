using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int levelCount;
    private int levelToLoad = 1;
    private AsyncOperation async;
    private bool loading = false;
    private bool levelComplete = true;
    private int count = 0;
    private Scene currScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        async = SceneManager.LoadSceneAsync(levelToLoad);
        levelCount = SceneManager.sceneCount;
    }

    // Update is called once per frame
    void Update()
    {
        currScene = SceneManager.GetActiveScene();

        if (loading == false)
        {
            LoadLevel();
            loading = true;
        }

        if (currScene.name != "PhilTestLevel 3")
        {
            if (async.progress >= 0.89f && levelComplete)
            {
                loading = false;
                levelComplete = false;
                async.allowSceneActivation = true;
            }
        }
    }

    public void LoadLevel()
    {
        if (async != null && count < levelCount)
        {
            count++;
            async = SceneManager.LoadSceneAsync(levelToLoad);
            levelToLoad++;
            async.allowSceneActivation = false;
        }
    }

    public void SetLevelComplete(bool status)
    {
        levelComplete = status;
    }
}
