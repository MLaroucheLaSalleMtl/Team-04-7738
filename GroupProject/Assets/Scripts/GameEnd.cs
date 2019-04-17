using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private GameManager code;
    [SerializeField] GameObject canvas;
    private float timer = 5;
    private bool once = false;

    // Start is called before the first frame update
    void Start()
    {
        code = FindObjectOfType<GameManager>();
        Invoke("LevelProgression", 5);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && timer <= 0 && !once)
        {
            once = true;
            code.SetLevelComplete();
        }
    }

    private void LevelProgression()
    {
        canvas.gameObject.SetActive(true);
    }
}
