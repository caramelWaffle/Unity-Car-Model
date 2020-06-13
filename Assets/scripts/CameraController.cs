using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float Y = 0f;
    public float B = 0f;
    public float Speed = 0.05f;
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (transform.position.y < 7)
            { 
                transform.Translate(0, Y, -Speed);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(transform.position.y > 1)
            {
                transform.Translate(0, B, Speed);

            }
        }

    }
}


