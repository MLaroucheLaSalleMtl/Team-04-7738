using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] Text scoreText;
    private GameManager code;
    private int[] score;

    // Start is called before the first frame update
    void Start()
    {
        code = FindObjectOfType<GameManager>();

        score = new int[3];
        score = code.GetAllScores();

        scoreText.text = "";

        for (int i = 0; i < score.Length; i++)
        {
            scoreText.text += $"Level {i + 1}        {score[i]}\n";
        }

        scoreText.text += $"\nTOTAL          {code.GetScore()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
