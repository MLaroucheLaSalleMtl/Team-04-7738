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

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        dashTime = DASH_DURATION;

        code = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Run();
            Jump();
            Dash();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Scene currScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.name);
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
            code.SetLevelComplete(true);
        }
       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Scene currScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.name);
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
            code.SetLevelComplete(true);
        }
    }
}
