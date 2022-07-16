using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.localPosition;
        if (Input.GetKeyDown(KeyCode.W))
        {
            pos.x--;
            transform.localPosition = pos;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            pos.x++;
            transform.localPosition = pos;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            pos.z--;
            transform.localPosition = pos;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            pos.z++;
            transform.localPosition = pos;
        }
    }
}
