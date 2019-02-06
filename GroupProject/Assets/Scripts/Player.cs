using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.5f;
    [SerializeField] float jumpHeight = 7f;
    [SerializeField] Image dashCDImage;
    private float dashDistance = 150f;

    Rigidbody2D myRigidbody;
    Animator myAnimator;

    private bool isGrounded = true;
    private bool canDash = true;
    private float dashTime;
    private float dashCoolDown = 3f;
    

    private const float DASH_DURATION = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        dashTime = DASH_DURATION;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        Dash();
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

        if (Input.GetButton("Fire2") && canDash)
        {
            canDash = false;
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
            //dashCDImage.fillAmount = ;

            Debug.Log(dashCoolDown);
            Debug.Log(dashCDImage.fillAmount);
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
}
