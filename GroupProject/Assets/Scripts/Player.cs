using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //GameManager
    private GameManager code;
    private LevelManager level;
    private Husk myHusk;
    private FinalBoss boss;

    //SFX
    [SerializeField] AudioClip[] sounds;
    AudioSource myAudio;
    private float footstepAudioPlayRate = 0.4f;

    //Player
    //[SerializeField] Text playerScore;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    bool canMove = true;
    float gracePeriod = 0.8f;

    //Movement
    [SerializeField] float movementSpeed = 3.5f;
    bool facingRight;

    //Jump
    [SerializeField] float jumpHeight = 7f;
    private bool isGrounded = true;

    //Health
    //[SerializeField] Slider hpSlider;

    //Mana
    //[SerializeField] Slider mpSlider;
    [SerializeField] float mpRegenInterval = 1f;
    private bool needsMana = false;

    //Dash
    //[SerializeField] Image dashCDImage;
    Vector2 dashForce;
    private const float DASH_DURATION = 0.4f;
    private const int DASH_MANA_COST = 15;
    private bool isDashing;
    private bool canDash = true;
    private float dashTime;
    private float dashCoolDown = 2.0f;
    private float dashDistance = 200f;
    private int dashTicks = 20;

    //Timer
    //[SerializeField] private Text timerText;
    //private float timer = 0;

    //Level
    bool sentToNextLevel = false;
    //private const int TIME_PENALTY = -25;
    //private const int DAMAGE_PENALTY = -5;
    //[SerializeField] GameObject scorePanel;
    //[SerializeField] Text scoreText;
    //[SerializeField] Text levelText;
    //private bool levelComplete;
    //private int levelCompleteScore = 5000;
    //private float levelEndTimer;

    //Menu
    //[SerializeField] GameObject menu;
    //private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();

        dashTime = DASH_DURATION;

        code = FindObjectOfType<GameManager>();
        level = FindObjectOfType<LevelManager>();
        myHusk = FindObjectOfType<Husk>();
        boss = FindObjectOfType<FinalBoss>();
    }

    //private void FixedUpdate()
    //{
    //    if (!level.LevelComplete())
    //    {
    //        if (!level.Paused())
    //        {
    //            if (canMove)
    //            {
    //                //if (Input.GetButtonDown("SwapForm"))
    //                //{
    //                //    SwapCharacters();
    //                //    //Destroy(gameObject);
    //                //    //level.LevelEnd();
    //                //}

    //                Run();
    //                Jump();
    //                Dash();
    //            }
    //        }
    //    }
    //}



    //Update is called once per frame
    void Update()
    {
        if (!level.LevelComplete())
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                level.LevelEnd();
            }
            if (Input.GetButtonDown("Cancel"))
            {
                level.Pause();
            }

            if (!level.Paused())
            {
                if (canMove)
                {
                    if (Input.GetButtonDown("SwapForm"))
                    {
                        SwapCharacters();
                    }

                    Run();
                    Jump();
                    Dash();
                }

                else
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

                RegenMana();
                DashCooldown();
            }
        }

        else
        {
            //if (Input.GetButtonDown("Jump") && !sentToNextLevel)
            //{
            //    sentToNextLevel = true;
            //    code.SetLevelComplete();
            //}

            if (!sentToNextLevel)
            {
                sentToNextLevel = true;
                code.Invoke("SetLevelComplete", 5);
            }

            //levelEndTimer -= Time.deltaTime;

            //if (levelEndTimer <= 0)
            //{
            //    code.SetLevelComplete();
            //}

        }
    }

    private void RegenMana()
    {
        if (level.MpSlider.value < 100)
        {
            needsMana = true;
        }

        if (needsMana)
        {
            mpRegenInterval -= Time.deltaTime;

            if (mpRegenInterval <= 0)
            {
                mpRegenInterval = 1f;
                level.MpSlider.value += 4;
            }
        }

    }

    void Run()
    {

        float movement = Input.GetAxis("Horizontal");//value between -1 to 1
        Vector2 playerVelocity = new Vector2(movement * movementSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        facingRight = (movement < 0) ? false : true;


        if (movement != 0)
        {
            myAnimator.SetBool("isWalking", true);
            GetComponent<SpriteRenderer>().flipX = !facingRight;

            footstepAudioPlayRate -= Time.deltaTime;

            if (footstepAudioPlayRate <= 0 && isGrounded)
            {
                myAudio.clip = sounds[0];
                myAudio.Play();
                footstepAudioPlayRate = 0.4f;
            }
        }

        else
        {
            myAnimator.SetBool("isWalking", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector2 jumpForce = new Vector2(myRigidbody.velocity.x, jumpHeight);
            myRigidbody.velocity = jumpForce;
            myAnimator.SetBool("Jump", true);
            myAudio.clip = sounds[1];
            myAudio.Play();
        }

        Debug.Log(myRigidbody.velocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.88f), Vector2.down);

        Debug.Log(groundInfo.distance);
        Debug.Log(groundInfo.collider.gameObject);

        if (groundInfo.distance < 0.1f && groundInfo.collider.gameObject.tag == "TileMap")
        {
            isGrounded = true;
            myAnimator.SetBool("Jump", false);
        }

        //    if (myRigidbody.velocity.y == 0)
        //{
        //    isGrounded = true;
        //    myAnimator.SetBool("Jump", false);
        //}

        else
        {
            isGrounded = false;
        }

    }

    void Dash()
    {
        if (Input.GetButton("UseAbility") && canDash && level.MpSlider.value >= DASH_MANA_COST)
        {
            canDash = false;
            level.MpSlider.value -= DASH_MANA_COST;
            myAnimator.SetBool("Dash", true);
            myAudio.clip = sounds[2];
            myAudio.Play();
            isDashing = true;

            //if (!GetComponent<SpriteRenderer>().flipX)
            //{
            //    dashForce = new Vector2(/*myRigidbody.velocity.x*/ +dashDistance, /*myRigidbody.velocity.y*/0);
            //}

            //else
            //{
            //    dashForce = new Vector2(/*myRigidbody.velocity.x*/ -dashDistance, /*myRigidbody.velocity.y*/0);
            //}

            dashForce = new Vector2(dashDistance, 0);
            myRigidbody.velocity = new Vector2(0, 0);
            //myRigidbody.velocity = dashForce;
            ////myRigidbody.AddForce(dashForce*500);
        }

        if (isDashing)
        {
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                myRigidbody.velocity = (dashForce / 20);
            }

            else
            {
                myRigidbody.velocity = (-dashForce / 20);
            }


            dashTicks--;

            if (dashTicks <= 0)
            {
                isDashing = false;
                dashTicks = 20;
            }
        }

        //if (!canDash)
        //{
        //    dashTime -= Time.deltaTime;
        //    dashCoolDown -= Time.deltaTime;
        //    level.DashCDImage.fillAmount = -dashCoolDown / 2.0f + 1;

        //    if (dashTime <= 0)
        //    {
        //        dashTime = DASH_DURATION;
        //        myAnimator.SetBool("Dash", false);
        //    }

        //    if (dashCoolDown <= 0)
        //    {
        //        dashCoolDown = 2.0f;
        //        canDash = true;
        //    }
        //}
    }

    private void DashCooldown()
    {
        if (!canDash)
        {
            dashTime -= Time.deltaTime;
            dashCoolDown -= Time.deltaTime;
            level.DashCDImage.fillAmount = -dashCoolDown / 2.0f + 1;

            if (dashTime <= 0)
            {
                dashTime = DASH_DURATION;
                myAnimator.SetBool("Dash", false);
            }

            if (dashCoolDown <= 0)
            {
                dashCoolDown = 2.0f;
                canDash = true;
            }
        }
    }

    private void TakeDamage(int damage)
    {
        if (!level.LevelComplete())
        {
            Vector2 knockback;

            if (canMove)
            {
                myAudio.clip = sounds[3];
                myAudio.Play();
                level.HpSlider.value -= damage;

                if (level.HpSlider.value <= 0)
                {
                    Die();
                }

                else
                {
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
            }

            canMove = false;
        }
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
        if (!level.LevelComplete())
        {
            if (collision.gameObject.tag == "Checkpoint")
            {
                myHusk.Checkpoint = transform.position;
            }

            if (!isDashing)
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
                    myAnimator.SetBool("isWalking", false);
                    myRigidbody.velocity = new Vector2(0, 0);
                    level.LevelEnd();
                }

                else if (collision.gameObject.tag == "Fireball")
                {
                    TakeDamage(20);
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!level.LevelComplete())
    //    {
    //        if (!isDashing)
    //        {
    //            if (collision.gameObject.tag == "Fireball")
    //            {
    //                TakeDamage(20);
    //            }
    //        }
    //    }
    //}



    public void Die()
    {
        if (!level.LevelComplete())
        {
            Time.timeScale = 1;
            transform.position = myHusk.Checkpoint;
            level.HpSlider.value = 100;
            level.MpSlider.value = 100;
            if (boss != null)
            {
                boss.RegenHealth();
            }
        }
    }

    private void SwapCharacters()
    {
        if (isGrounded && canDash)
        {
            myHusk.SwapCharacters(this, GetComponent<SpriteRenderer>().flipX);
            level.DashCDImage.fillAmount = 100;
            Destroy(gameObject);
        }
    }

    //private void LevelComplete()
    //{
    //    int score = levelCompleteScore + (int)((int)timer * TIME_PENALTY + (100 - hpSlider.value) * DAMAGE_PENALTY);

    //    myAnimator.SetBool("isWalking", false);
    //    myRigidbody.velocity = new Vector2(0, 0);
    //    code.SaveScore(score);
    //    scorePanel.gameObject.SetActive(true);
    //    scoreText.text = $"TIME PENALTY: {(int)timer} x {TIME_PENALTY} = {(int)timer * TIME_PENALTY}\n" +
    //                     $"DAMAGE PENALTY: {100 - hpSlider.value} x {DAMAGE_PENALTY} = {(100 - hpSlider.value) * DAMAGE_PENALTY}\n" +
    //                     $"SCORE = {score}\n" +
    //                     $"TOTAL SCORE = {code.GetScore()}";
    //    levelComplete = true;

    //    //code.Invoke("SetLevelComplete", 4);
    //}
}
