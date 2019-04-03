﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Husk : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerTransformed;
    private GameObject clone;
    private LevelManager level;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        clone = Instantiate(player, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = clone.transform.position;
    }

    public void SwapCharacters<T>(T myPlayer, bool flipped)
    {
        if (myPlayer is Player)
        {
            level.DashMenu.SetActive(false);
            clone = Instantiate(playerTransformed, transform.position, transform.rotation);

            if (flipped)
            {
                clone.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        else
        {
            level.DashMenu.SetActive(true);
            clone = Instantiate(player, transform.position, transform.rotation);

            if (flipped)
            {
                clone.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

}
