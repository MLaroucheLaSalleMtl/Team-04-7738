using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTransformed : MonoBehaviour
{
    //GameManager
    private GameManager code;
    private LevelManager level;
    private Husk myHusk;

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
    private bool isGrounded = true;

    //Health
    //[SerializeField] Slider hpSlider;

    //Mana
    //[SerializeField] Slider mpSlider;
    [SerializeField] float mpRegenInterval = 1f;
    private bool needsMana = false;

    //Attack
    private const int ATTACK_MANA_COST = 10;
    private bool canAttack = true;
    private float attackTime = 0.5f;
    [SerializeField] private Projectile projectile;

    //Level
    bool sentToNextLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();

        code = FindObjectOfType<GameManager>();
        level = FindObjectOfType<LevelManager>();
        myHusk = FindObjectOfType<Husk>();

        //projectile = FindObjectOfType<Projectile>();
    }

    private void FixedUpdate()
    {
        if (!level.LevelComplete())
        {
            if (!level.Paused())
            {
                if (canMove)
                {
                    Run();
                    Attack();
                }

                RegenMana();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwapForm"))
        {
            SwapCharacters();
            //Destroy(gameObject);
            //level.LevelEnd();
        }

        if (!level.LevelComplete())
        {
            if (Input.GetButtonDown("Cancel"))
            {
                level.Pause();
            }

            if (!level.Paused())
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

        else
        {
            if (Input.GetButtonDown("Jump") && !sentToNextLevel)
            {
                sentToNextLevel = true;
                code.SetLevelComplete();
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
                level.MpSlider.value += 2;//twice as fast as first form
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
            //myAnimator.SetBool("isWalking", true);
            GetComponent<SpriteRenderer>().flipX = !facingRight;

            footstepAudioPlayRate -= Time.deltaTime;

            if (footstepAudioPlayRate <= 0 && isGrounded)
            {
                //myAudio.clip = sounds[0];
                //myAudio.Play();
                footstepAudioPlayRate = 0.4f;
            }
        }

        else
        {
            //myAnimator.SetBool("isWalking", false);
        }
    }

    private void TakeDamage(int damage)
    {
        if (!level.LevelComplete())
        {
            Vector2 knockback;

            if (canMove)
            {
                //myAudio.clip = sounds[3];
                //myAudio.Play();
                level.HpSlider.value -= damage;

                if (level.HpSlider.value <= 0)
                {
                    Die();
                }

                if (!GetComponent<SpriteRenderer>().flipX)//facing right
                {
                    knockback = new Vector2(-3500, 3500);
                }

                else//facing left
                {
                    knockback = new Vector2(3500, 3500);
                }

                myRigidbody.velocity = new Vector2(0, 0);
                myRigidbody.AddForce(knockback);
            }

            canMove = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!level.LevelComplete())
        {
            if (collision.gameObject.tag == "Death")
            {
                Die();
            }

            else if (collision.gameObject.tag == "Spike")
            {
                TakeDamage(5);//half damage
            }

            else if (collision.gameObject.tag == "Enemy")
            {
                TakeDamage(8);//half damage rounded up
            }

            else if (collision.gameObject.tag == "Finish")
            {
                //myAnimator.SetBool("isWalking", false);
                myRigidbody.velocity = new Vector2(0, 0);
                level.LevelEnd();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!level.LevelComplete())
        {
            if (!isDashing)
            {
                if (collision.gameObject.tag == "Fireball")
                {
                    TakeDamage(20);
                }
            }
        }
    }

    public void Die()
    {
        if (!level.LevelComplete())
        {
            Time.timeScale = 1;
            Scene currScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.name);

        }
    }

    public void Quit()
    {
        Time.timeScale = 1;
        code.MainMenu();
    }

    private void SwapCharacters()
    {
        if (isGrounded)
        {
            myHusk.SwapCharacters(this, GetComponent<SpriteRenderer>().flipX);
            level.DashCDImage.fillAmount = 100;
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire3") && canAttack && level.MpSlider.value >= ATTACK_MANA_COST)
        {
            

            canAttack = false;
            level.MpSlider.value -= ATTACK_MANA_COST;
            myAnimator.SetBool("IsAttacking", true);
            //myAudio.clip = sounds[2];
            //myAudio.Play();

            //if (!GetComponent<SpriteRenderer>().flipX)//right
            //{
            //    proj = Instantiate(projectile, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
            //}

            //else//left
            //{
            //    proj = Instantiate(projectile, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
            //    proj.ChangeDirection();
            //}

        }

        if (!canAttack)
        {
            attackTime -= Time.deltaTime;

            if (attackTime <= 0)
            {
                myAnimator.SetBool("IsAttacking", false);
                attackTime = 0.5f;
                canAttack = true;

                Projectile proj;

                if (!GetComponent<SpriteRenderer>().flipX)//right
                {
                    proj = Instantiate(projectile, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
                }

                else//left
                {
                    proj = Instantiate(projectile, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
                    proj.ChangeDirection();
                }
            }
        }
    }
}
