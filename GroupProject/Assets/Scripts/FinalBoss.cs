using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private GameObject fireballPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Cast();
        }
    }

    void Cast()
    {
        Instantiate(fireballPrefab, startingPoint.position, startingPoint.rotation);
    }
}
