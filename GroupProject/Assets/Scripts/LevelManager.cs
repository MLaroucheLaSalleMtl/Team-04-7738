using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private GameManager code;

    //Player
    [SerializeField] Text playerScore;

    //Health
    [SerializeField] Slider hpSlider;

    //Mana
    [SerializeField] Slider mpSlider;

    //Timer
    [SerializeField] private Text timerText;
    private float timer = 0;

    //Cooldowns
    [SerializeField] GameObject dashMenu;
    [SerializeField] Image dashCDImage;

    //Level
    private const int TIME_PENALTY = -25;
    private const int DAMAGE_PENALTY = -5;
    [SerializeField] GameObject scorePanel;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;
    private bool levelComplete;
    private int levelCompleteScore = 5000;
    private float levelEndTimer;

    //Menu
    [SerializeField] GameObject menu;
    private bool paused = false;

    public Slider MpSlider { get => mpSlider; set => mpSlider = value; }
    public Slider HpSlider { get => hpSlider; set => hpSlider = value; }
    public Image DashCDImage { get => dashCDImage; set => dashCDImage = value; }
    public GameObject DashMenu { get => dashMenu; set => dashMenu = value; }

    // Start is called before the first frame update
    void Start()
    {
        code = FindObjectOfType<GameManager>();
        scorePanel.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);

        playerScore.text = "SCORE: " + code.GetScore().ToString("D5");
        levelText.text = $"LEVEL\n {code.GetLevel()}";
        levelComplete = false;
        levelEndTimer = 4;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!levelComplete)
        {
            Timer();
        }
    }

    public bool LevelComplete()
    {
        return levelComplete;
    }

    public bool Paused()
    {
        return paused;
    }

    public void Pause()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0;
        }

        else
        {
            paused = false;
            Time.timeScale = 1;
        }

        menu.gameObject.SetActive(paused);
    }

    void Timer()
    {
        timer += Time.deltaTime;
        timerText.text = ((int)timer).ToString("D3");
    }

    public void LevelEnd()
    {
        int score = levelCompleteScore + (int)((int)timer * TIME_PENALTY + (100 - hpSlider.value) * DAMAGE_PENALTY);

        code.SaveScore(score);
        scorePanel.gameObject.SetActive(true);
        scoreText.text = $"TIME PENALTY: {(int)timer} x {TIME_PENALTY} = {(int)timer * TIME_PENALTY}\n" +
                         $"DAMAGE PENALTY: {100 - hpSlider.value} x {DAMAGE_PENALTY} = {(100 - hpSlider.value) * DAMAGE_PENALTY}\n" +
                         $"SCORE = {score}\n" +
                         $"TOTAL SCORE = {code.GetScore()}";
        levelComplete = true;

        //code.Invoke("SetLevelComplete", 4);
    }
}
