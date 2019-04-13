﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireskull : MonoBehaviour
{
    [SerializeField] private float travelSpeed = 0.175f;
    [SerializeField] private float lifeTime = 2.0f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x - travelSpeed, transform.position.y);
    }
}
