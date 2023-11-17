using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAtPostion : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0.006387268f, 7.614214e-10f, 1.522843e-09f);
    public float scaleFactor = 2.0f;

    void Update()
    {
       /* // Check if the GameObject is at the target position
        if (transform.position == targetPosition)
        {
            // Scale the GameObject at the target position
            //  transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            transform.localScale = Vector3.one;
        }
        else
        {
            // Reset the scale if not at the target position
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }*/
    }
}
