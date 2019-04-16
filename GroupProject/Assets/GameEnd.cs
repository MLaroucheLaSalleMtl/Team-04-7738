using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private GameManager code;

    // Start is called before the first frame update
    void Start()
    {
        code = FindObjectOfType<GameManager>();
        code.Invoke("SetLevelComplete", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
