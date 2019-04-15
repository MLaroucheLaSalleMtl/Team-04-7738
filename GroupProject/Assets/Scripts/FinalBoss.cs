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

    //Spawns and Prefabs
    [SerializeField] private Transform fireskullSpawn;
    [SerializeField] private Transform frostbiteSpawn1;
    [SerializeField] private Transform frostbiteSpawn2;
    [SerializeField] private Transform frostbiteSpawn3;
    [SerializeField] private Transform frostbiteSpawn4;
    [SerializeField] private GameObject fireskullPrefab;
    [SerializeField] private GameObject frostbitePrefab;

    //Attack
    private Animator anim;
    [SerializeField] private float timer = 0f;
    private int random; 

    void Start()
    {
        anim = GetComponent<Animator>();
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

        timer -= Time.deltaTime;

        if (timer <= 2.5f)
        {
            anim.SetBool("Cast", false);
        }

        if (timer <= 0f)
        {
            timer = 3.5f;
            random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    FireskullCast();
                    break;
                case 1:
                    FrostbiteCast();
                    break;
                default:
                    break;
            }
        }

        //if (timer > 0f)
        //{
        //    timer -= Time.deltaTime;
        //    if (timer <= 0f)
        //    {
        //        switch (random)
        //        {
        //            case 0:
        //                FireskullCast();
        //                break;
        //            case 1:
        //                FrostbiteCast();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
    }

    void FireskullCast()
    {
        anim.SetBool("Cast", true);
        Instantiate(fireskullPrefab, fireskullSpawn.position, fireskullSpawn.rotation);
        //timer = 0f;
    }

    void FrostbiteCast()
    {
        anim.SetBool("Cast", true);
        Instantiate(frostbitePrefab, frostbiteSpawn1.position, frostbiteSpawn1.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn2.position, frostbiteSpawn2.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn3.position, frostbiteSpawn3.rotation);
        Instantiate(frostbitePrefab, frostbiteSpawn4.position, frostbiteSpawn4.rotation);
        //timer = 0f;
    }

    public void TakeDamage()
    {

    }
}
