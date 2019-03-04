using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private GameManager code;

    //Player
    [SerializeField] Text playerScore;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    bool canMove = true;
    float gracePeriod = 0.8f;

    //Movement
    [SerializeField] float movementSpeed = 3.5f;

    //Jump
    [SerializeField] float jumpHeight = 7f;

    private bool isGrounded = true;

    //Health
    [SerializeField] Slider hpSlider;

    //Mana
    [SerializeField] Slider mpSlider;
    [SerializeField] float mpRegenInterval = 1f;

    private bool needsMana = false;

    //Dash
    [SerializeField] Image dashCDImage;

    private const float DASH_DURATION = 0.4f;
    private const int DASH_MANA_COST = 15;

    private bool canDash = true;
    private float dashTime;
    private float dashCoolDown = 3f;
    private float dashDistance = 200f;

    //Timer
    [SerializeField] private Text timerText;
    private float timer = 0;

    //Level
    private const int TIME_PENALTY = -25;
    private const int DAMAGE_PENALTY = -5;
    [SerializeField] GameObject scorePanel;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;
    private bool levelComplete = false;
    private int levelCompleteScore = 5000;

    //Menu
    [SerializeField] GameObject menu;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        scorePanel.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        dashTime = DASH_DURATION;

        code = FindObjectOfType<GameManager>();
        playerScore.text = "SCORE: " + code.GetScore().ToString("D5");
        levelText.text = $"LEVEL\n {code.GetLevel()}";
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            if (!levelComplete)
            {
                if (canMove)
                {
                    Run();
                    Jump();
                    Dash();
                }

                Timer();
                RegenMana();
            }
        }
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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }

        if (!paused)
        {
            if (!levelComplete)
            {
                if (!canMove)
                {
                    gracePeriod -= Time.deltaTime;

                    if (((int)(gracePeriod * 100) % 2) == 1)
                    {
                        GetComponent<SpriteRenderer>().color = Color.black;
                    }

                    else if (((int)(gracePeriod * 100) % 2) == 0)
                    {
                        GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    if (gracePeriod <= 0)
                    {
                        GetComponent<SpriteRenderer>().color = Color.white;
                        canMove = true;
                        gracePeriod = 0.8f;
                    }
                }
            }
        }
    }

    private void RegenMana()
    {
        if (mpSlider.value < 100)
        {
            needsMana = true;
        }

        if (needsMana)
        {
            mpRegenInterval -= Time.deltaTime;

            if (mpRegenInterval <= 0)
            {
                mpRegenInterval = 1f;
                mpSlider.value += 1;
            }
        }
        
    }

    void Run()
    {
        bool facingRight = true;

        float movement = Input.GetAxis("Horizontal");//value between -1 to 1
        Vector2 playerVelocity = new Vector2(movement * movementSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        facingRight = (movement < 0) ? false : true;


        if (movement != 0)
        {
            myAnimator.SetBool("isWalking", true);
            GetComponent<SpriteRenderer>().flipX = !facingRight;
        }

        else
        {
            myAnimator.SetBool("isWalking", false);
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            Vector2 jumpForce = new Vector2(myRigidbody.velocity.x, jumpHeight);
            myRigidbody.velocity = jumpForce;
            myAnimator.SetBool("Jump", true);
        }

        if (myRigidbody.velocity.y == 0)
        {
            isGrounded = true;
            myAnimator.SetBool("Jump", false);
        }

        else
        {
            isGrounded = false;
        }

    }

    void Dash()
    {
        Vector2 dashForce;

        if (Input.GetButton("Fire3") && canDash && mpSlider.value >= DASH_MANA_COST)
        {
            canDash = false;
            mpSlider.value -= DASH_MANA_COST;
            myAnimator.SetBool("Dash", true);

            if (!GetComponent<SpriteRenderer>().flipX)
            {
                dashForce = new Vector2(myRigidbody.velocity.x + dashDistance, myRigidbody.velocity.y);
                
            }

            else
            {
                dashForce = new Vector2(myRigidbody.velocity.x - dashDistance, myRigidbody.velocity.y);
            }

            myRigidbody.velocity = dashForce;
        }

        if (!canDash)
        {
            dashTime -= Time.deltaTime;
            dashCoolDown -= Time.deltaTime;
            dashCDImage.fillAmount = -dashCoolDown/3 + 1;

            if (dashTime <= 0)
            {
                dashTime = DASH_DURATION;
                myAnimator.SetBool("Dash", false);
            }

            if (dashCoolDown <= 0)
            {
                dashCoolDown = 3f;
                canDash = true;
                
            }
        }
    }

    private void TakeDamage(int damage)
    {
        Vector2 knockback;

        if (canMove)
        {
            hpSlider.value -= damage;

            if (hpSlider.value <= 0)
            {
                Scene currScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currScene.name);
            }

            if (!GetComponent<SpriteRenderer>().flipX)//facing right
            {
                knockback = new Vector2(-5000, 5000);
            }

            else//facing left
            {
                knockback = new Vector2(5000, 5000);
            }

            myRigidbody.velocity = new Vector2(0, 0);
            myRigidbody.AddForce(knockback);
        }
        
        canMove = false;
    }

    void Timer()
    {
        timer += Time.deltaTime;
        timerText.text = ((int)timer).ToString("D3");
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Death")
    //    {
    //        Scene currScene = SceneManager.GetActiveScene();
    //        SceneManager.LoadScene(currScene.name);
    //    }

    //    else if (collision.gameObject.tag == "Spike")
    //    {
    //        TakeDamage(10);
    //    }

    //    else if (collision.gameObject.tag == "Enemy")
    //    {
    //        TakeDamage(15);
    //    }

    //    else if (collision.gameObject.tag == "Finish")
    //    {
    //        code.SetLevelComplete(true);
    //    }

    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!levelComplete)
        {
            if (collision.gameObject.tag == "Death")
            {
                Die();
            }

            else if (collision.gameObject.tag == "Spike")
            {
                TakeDamage(10);
            }

            else if (collision.gameObject.tag == "Enemy")
            {
                TakeDamage(15);
            }

            else if (collision.gameObject.tag == "Finish")
            {
                int score = levelCompleteScore + (int)((int)timer * TIME_PENALTY + (100 - hpSlider.value) * DAMAGE_PENALTY);

                myAnimator.SetBool("isWalking", false);
                myRigidbody.velocity = new Vector2(0, 0);
                code.SaveScore(score);
                scorePanel.gameObject.SetActive(true);
                scoreText.text = $"TIME PENALTY: {(int)timer} x {TIME_PENALTY} = {(int)timer * TIME_PENALTY}\n" +
                                 $"DAMAGE PENALTY: {100 - hpSlider.value} x {DAMAGE_PENALTY} = {(100 - hpSlider.value) * DAMAGE_PENALTY}\n" +
                                 $"SCORE = {score}\n" +
                                 $"TOTAL SCORE = {code.GetScore()}";
                levelComplete = true;

                code.Invoke("SetLevelComplete", 3);
            }
        }
    }

    public void Die()
    {
        Time.timeScale = 1;
        Scene currScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currScene.name);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        code.MainMenu();
    }
}
