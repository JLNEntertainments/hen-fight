using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationInPosition : MonoBehaviour
{
    public Animator animator;
    public float thresholdX = 5.0f;
    public float thresholdY = 5.0f;
    public float thresholdZ = 5.0f;

    void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = transform.position;

        // Check conditions based on x, y, and z positions
        if (currentPosition.x > thresholdX)
        {
            animator.SetFloat("PosX", 1.0f); // Set the x position parameter
        }
        else
        {
            animator.SetFloat("PosX", 0.0f);
        }

        if (currentPosition.y > thresholdY)
        {
            animator.SetFloat("PosY", 1.0f); // Set the y position parameter
        }
        else
        {
            animator.SetFloat("PosY", 0.0f);
        }

        if (currentPosition.z > thresholdZ)
        {
            animator.SetFloat("PosZ", 1.0f); // Set the z position parameter
        }
        else
        {
            animator.SetFloat("PosZ", 0.0f);
        }
    }
}
