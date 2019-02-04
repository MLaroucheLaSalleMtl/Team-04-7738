using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.5f;
    [SerializeField] float jumpHeight = 7f;


    Rigidbody2D myRigidbody;
    Animator myAnimator;

    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
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
}
