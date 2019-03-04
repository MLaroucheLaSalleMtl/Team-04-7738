using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int levelCount;
    private int levelToLoad = 1;
    private AsyncOperation async;
    private bool loading = false;
    private bool levelComplete = true;
    private int count = 0;
    private Scene currScene;
    private int levelIndex = 0;
    private int[] score;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        async = SceneManager.LoadSceneAsync(levelToLoad);
        levelCount = 6;
        score = new int[3];
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

        if (currScene.name != "End")
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
        if (async != null && count < levelCount - 1)
        {
            count++;
            async = SceneManager.LoadSceneAsync(levelToLoad);
            levelToLoad++;
            async.allowSceneActivation = false;
        }
    }

    public void SetLevelComplete()
    {
        levelComplete = true;
    }

    public void SaveScore(int score)
    {
        this.score[levelIndex] = score;
        levelIndex++;
    }

    public int GetScore()
    {
        int totalScore = 0;

        if (levelIndex != 0)
        {
            for (int i = 0; i < levelIndex; i++)
            {
                if (score[i] > 0)
                {
                    totalScore += score[i];
                }
            }
        }
        return totalScore;
    }

    public int[] GetAllScores()
    {
        return score;
    }

    public int GetLevel()
    {
        return levelIndex + 1;
    }

    public void MainMenu()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
