using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAtPostion : MonoBehaviour
{
    // Set the original and different scale values
    public Vector3 originalScale = new Vector3(1f, 1f, 1f);
    public Vector3 scaledPosition = new Vector3(5f, 2f, 3f);



    //  public Vector3 oldTargetPosition = new Vector3(0f, 0f, 0f);

    void Update()
    {
        // Check the current position of the object
        Vector3 currentPosition = transform.position;

        // Check if the object is at a specific position
        if (currentPosition.x >= scaledPosition.x && currentPosition.y >= scaledPosition.y && currentPosition.z >= scaledPosition.z)
        {
            // Change the scale of the game object
            ChangeScale(scaledPosition);
        }
        else
        {
            // Reset the scale to the original scale if not at the desired position
            ChangeScale(originalScale);
        }
    }

    void ChangeScale(Vector3 scale)
    {
        // Access the Transform component of the game object
        Transform objTransform = transform;

        // Modify the localScale property to change the scale
        objTransform.localScale = scale;
    }
}
