using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndRotate : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private Vector2 touchStartPos;
    public Vector3 newRotation = new Vector3(0f, 0f, 0f); // Set the desired new position in the Inspector

    void OnEnable()
    {
        // Change the position of the GameObject when it is activated
        transform.rotation = Quaternion.Euler(newRotation);
    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {

            // Get the mouse movement
            float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // Rotate the object based on mouse movement
                float rotateX = -mouseY * rotationSpeed * Time.deltaTime;
                float rotateY = mouseX * rotationSpeed * Time.deltaTime;

                // Apply rotation to the object
                transform.Rotate(Vector3.up, -rotateY, Space.World);
                //transform.Rotate(Vector3.right, rotateX, Space.World);

            }
        }
    }

