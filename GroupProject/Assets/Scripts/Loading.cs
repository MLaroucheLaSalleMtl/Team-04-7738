using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Text loading;
    private int count = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (count <= 33)
        {
            loading.text = "LOADING   ";
            count++;
        }

        else if (count > 33 && count <= 66)
        {
            loading.text = "LOADING.  ";
            count++;
        }

        else if (count > 66 && count <= 100)
        {
            loading.text = "LOADING.. ";
            count++;
        }

        else if (count > 100 && count <= 133)
        {
            loading.text = "LOADING...";
            count++;
        }

        else
        {
            count = 0;
        }
    }
}
