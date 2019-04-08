using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    private bool movingRight = true;
    public Transform groundDetection;
    private float deathAnim = 0.667f;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Die()
    {
        if (movingRight)
        {
            transform.position += new Vector3(1f, 0.5f, 0);
        }
        else
        {
            transform.position += new Vector3(-1f, 0.5f, 0);
        }
        
        dead = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;

            GetComponent<Animator>().SetBool("Death", true);
            deathAnim -= Time.deltaTime;
            if (deathAnim <= 0)
            {
                Destroy(gameObject);
            }
        }

        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f);
            if (groundInfo.collider == false)
            {
                if (movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;

                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;

                }
            }
        }
    }
}
