using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrientation : MonoBehaviour
{
    [SerializeField]
    private Transform enemyTransform; // Reference to the enemy's Transform
    private float rotationSpeed = 5f; // Speed of rotation towards the enemy
    int playerZOrientation = 1;
    [SerializeField]
    private float speed = 2f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        RotateTowardsEnemy();
    }

    void RotateTowardsEnemy()
    {
        // Ensure enemyTransform is assigned
        if (enemyTransform != null)
        {
            Vector3 directionToEnemy = enemyTransform.position - transform.position;
            directionToEnemy.y = 0; // Keep the rotation only in the Y axis
            if (transform.position.x < enemyTransform.position.x)
            {
                playerZOrientation = -1;
            }
            else
            {
                playerZOrientation = 1;
            }
            if (directionToEnemy != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);

                // Increase the speed value to rotate faster
                float increasedSpeed = speed * 2f; // Example: double the speed

                // Modify the interpolation factor to maintain a faster rotation speed throughout
                float interpolationFactor = Mathf.Pow(Time.deltaTime * increasedSpeed, 0.6f); // Using a power less than 1 to reduce cushioning

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, interpolationFactor);
            }
        }
    }
}
