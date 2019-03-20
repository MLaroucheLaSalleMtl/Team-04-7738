using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;

    private float followSpeed;
    private Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        followSpeed = 0.04f;
        vel = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed) + new Vector3(0, 0, -10);
        //transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref vel, followSpeed) + new Vector3(0, 0, -10);
    }
}
