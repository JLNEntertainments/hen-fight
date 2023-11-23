using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoominZoomOyut : MonoBehaviour
{
    Vector3 centerPosition;//positon of object you want to zoom in and out of
    float MaxZoom = 3f;

    void Update()
    {
        //mouse wheel chaned
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera();
        }
    }

    public void ZoomCamera()
    {
        Vector3 newPosition = Vector3.MoveTowards(
            transform.position, centerPosition, Input.mouseScrollDelta.y);

        if (newPosition.y >= (centerPosition.y + MaxZoom))
        {
            transform.position = newPosition;
        }
    }
}
