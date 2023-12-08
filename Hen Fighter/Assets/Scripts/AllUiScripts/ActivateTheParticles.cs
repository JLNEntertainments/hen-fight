using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTheParticles : MonoBehaviour
{
    /*public GameObject objectToActivate;
    public float activationThresholdX = 5.0f;
    public float activationThresholdY = 5.0f;
    public float activationThresholdZ = 5.0f;

    void Update()
    {
        // Get the current position of the object
        Vector3 currentPosition = transform.position;

        // Check conditions based on x, y, and z positions
        if (currentPosition.x < activationThresholdX &&
            currentPosition.y < activationThresholdY &&
            currentPosition.z < activationThresholdZ)
        {
            // Activate the GameObject
            if (objectToActivate != null && !objectToActivate.activeSelf)
            {
                objectToActivate.SetActive(true);
            }
        }
        else
        {
            // Deactivate the GameObject
            if (objectToActivate != null && objectToActivate.activeSelf)
            {
                objectToActivate.SetActive(false);
            }
        }

    }*/

    public ParticleSystem particleSystem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Instantiate(particleSystem, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

   
}

