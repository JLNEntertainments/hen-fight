using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInZoomOutForModel : MonoBehaviour
{
    public float ZoomCahnge;
    public float SmootChanges;
    public float MinSize, MaxSize;

    private Camera Cam;


    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
            Cam.orthographicSize -= ZoomCahnge * Time.deltaTime * SmootChanges;
        if(Input.mouseScrollDelta.y<0)
            Cam.orthographicSize += ZoomCahnge * Time.deltaTime * SmootChanges;
        Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize, MinSize, MaxSize);
    }
}
