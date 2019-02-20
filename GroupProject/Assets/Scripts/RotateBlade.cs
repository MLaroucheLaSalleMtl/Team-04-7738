using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateBlade : MonoBehaviour
{
    public float rotationSpeed;

    void Start()
    {

    }


    void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Scene currScene = SceneManager.GetActiveScene();
    //    SceneManager.LoadScene(currScene.name);
    //}
}
