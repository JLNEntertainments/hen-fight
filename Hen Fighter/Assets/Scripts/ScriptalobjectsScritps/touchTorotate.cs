using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class touchTorotate : MonoBehaviour
{
    private Vector3 lastpos, currpos;
    private float rotationspeed = 0.001f;

     void Start()
    {
        lastpos = Input.mousePosition;
    }

     void Update()
    {
        if (Input.GetMouseButton(0))
        {

            currpos = Input.mousePosition;
            Vector3 offset = currpos - lastpos;
            transform.RotateAround(transform.position, Vector3.up, offset.x * rotationspeed);
          

        }
        else
        {
            lastpos = Input.mousePosition;
        }
    }
}