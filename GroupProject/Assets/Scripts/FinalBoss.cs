using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    //Health
    [SerializeField] private float health;
    [SerializeField] private float heartsAmount;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private bool bossAlive = true;

    //Spawns and Prefabs
    [SerializeField] private Transform fireskullSpawn;
    [SerializeField] private Transform frostbiteSpawn1;
    [SerializeField] private Transform frostbiteSpawn2;
    [SerializeField] private Transform frostbiteSpawn3;
    [SerializeField] private Transform frostbiteSpawn4;
    [SerializeField] private Transform lightningSpawn1;
    [SerializeField] private Transform lightningSpawn2;
    [SerializeField] private Transform lightningSpawn3;
    [SerializeField] private Transform lightningSpawn4;
    [SerializeField] private GameObject fireskullPrefab;
    [SerializeField] private GameObject frostbitePrefab;
    [SerializeField] private GameObject lightningPrefab;

    //Attack
    private Animator anim;
    private float timer = 2.5f;
    private int random;

    //SFX
    private AudioSource audioSource;
    [SerializeField] AudioClip fireSkullSFX, frostbiteSFX, lightningSFX, death;

    //Level
    private GameManager code;
    private LevelManager level;
    private bool sentToNextLevel = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        code = FindObjectOfType<GameManager>();
        level = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        if (level.LevelComplete())
        {
            if (!sentToNextLevel)
            {
                sentToNextLevel = true;
                code.Invoke("SetLevelComplete", 5);
            }
        }


        if (bossAlive)
        {
            timer -= Time.deltaTime;

            if (timer <= 1.5f)
            {
                anim.SetBool("Cast", false);
            }

            if (timer <= 0f)
            {
                timer = 2.5f;
                random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        FireskullCast();
                        break;
                    case 1:
                        FrostbiteCast();
                        break;
                    case 2:
                        Invoke("LightningStrike", 1);
                        break;
                    default:
                        FireskullCast();
                        break;
                }
            }
        }

        else
        {
            FindObjectOfType<Camera>().GetComponent<AudioSource>().volume -= 0.005f;
        }
    }

    void FireskullCast()
    {
        anim.SetBool("Cast", true);
        Instantiate(fireskullPrefab, fireskullSpawn.position, fireskullSpawn.rotation);
        FireskullSFX();
    }

    void FrostbiteCast()
    {
        anim.SetBool("Cast", true);
        Instantiate(frostbitePrefab, frostbiteSpawn1.position, frostbiteSpawn1.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn2.position, frostbiteSpawn2.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn3.position, frostbiteSpawn3.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn4.position, frostbiteSpawn4.rotation);
        FrostbiteSFX();
    }

    void LightningStrike()
    {
        anim.SetBool("Cast", true);
        Instantiate(lightningPrefab, lightningSpawn1.position, lightningSpawn1.rotation);
        Instantiate(lightningPrefab, lightningSpawn2.position, lightningSpawn2.rotation);
        Instantiate(lightningPrefab, lightningSpawn3.position, lightningSpawn3.rotation);
        Instantiate(lightningPrefab, lightningSpawn4.position, lightningSpawn4.rotation);
        LightningSFX();
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            audioSource.PlayOneShot(death);
            anim.SetBool("Dead", true);
            bossAlive = false;
            level.Invoke("LevelEnd", 3);
        }

    }

    private void FireskullSFX() {
        audioSource.PlayOneShot(fireSkullSFX);
    }

    private void FrostbiteSFX() {
        audioSource.PlayOneShot(frostbiteSFX);
    }

    private void LightningSFX()
    {
        audioSource.PlayOneShot(lightningSFX);
    }

    public void RegenHealth()
    {
        health = 20;
    }
}
