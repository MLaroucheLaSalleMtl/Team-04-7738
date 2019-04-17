using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private float travelSpeed = 0.175f;
    private float lifeTime = 3f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + travelSpeed);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
