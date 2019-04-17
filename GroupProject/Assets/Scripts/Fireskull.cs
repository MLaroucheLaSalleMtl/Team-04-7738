using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireskull : MonoBehaviour
{
    private float travelSpeed = 0.175f;
    private float lifeTime = 5.0f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x - travelSpeed, transform.position.y);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
