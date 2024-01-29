using System.Collections;
using UnityEngine;

public class PlayerGamePlayManager : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    private Animator playerAnimator;
    private Rigidbody playerRb;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpForce = 4f;
    [SerializeField]
    private float walkThreshold = 0.3f; // Threshold to differentiate between walk and idle
    [SerializeField]
    private float runThreshold = 0.8f; // Threshold to differentiate between run and walk

    [SerializeField]
    private Transform enemyTransform; // Reference to the enemy's Transform
    private float rotationSpeed = 5f; // Speed of rotation towards the enemy
    int playerZOrientation = 1;

    private int attackIntensity = 1;
    private float comboTimer = 0f;
    private float comboMaxTime = 1.5f; // Time window for next attack button press

    public bool isPlayerControlled = true;

    private float airTime = 0.0f;
    private const float maxAirTime = 0.4f;

    float retreatMultiplier = 5.0f; // this may vary based on the level of impact
    private float distanceToEnemy;
    bool isUnderAttack = false;
    bool canRetaliate = true;
    float retreatSpeed = 2.0f;
    float safeDistance = 3.0f;
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isPlayerControlled)
        {
            HandlePlayerMovement();
            // Existing player input handling
        }
        else
        {
            HandleAIMovement();
            // AI behavior update
        }

        RotateTowardsEnemy();
        distanceToEnemy = CalculateDistanceToEnemy();

        // Combo timing logic
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }

        if (!IsGrounded())
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime = 0.0f;
        }
        // Check if the character has been in the air for too long
        if (airTime >= maxAirTime)
        {
            EnsureGrounding();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == enemyTransform.gameObject)
        {
            Rigidbody enemyRb = enemyTransform.GetComponent<Rigidbody>();

            // Calculate and apply the initial collision force
            Vector3 collisionForce = CalculateCollisionForce();
            enemyRb.AddForce(collisionForce, ForceMode.Impulse);

            // Additional logic, such as triggering animations or starting the retreat coroutine
            StartCoroutine(EnemyAdditionalPushback());
        }
    }

    IEnumerator EnemyAdditionalPushback()
    {
        float pushbackDuration = 0.2f; // Duration of the pushback
        float pushbackSpeed = 0.8f; // Speed of the pushback
        float maxAcceptableDistance = 4f; // Set your maximum acceptable distance
        Vector3 pushbackDirection = (enemyTransform.position - transform.position).normalized;

        float startTime = Time.time;
        while (Time.time < startTime + pushbackDuration)
        {
            Vector3 proposedNewPosition = enemyTransform.position + pushbackDirection * pushbackSpeed * Time.deltaTime;

            // Check if the proposed new position is within the maximum acceptable distance
            if (Vector3.Distance(transform.position, proposedNewPosition) <= maxAcceptableDistance)
            {
                enemyTransform.position = proposedNewPosition;
            }
            else
            {
                // Break the loop if the distance exceeds the maximum acceptable limit
                break;
            }
            yield return null;
        }
    }

    /*Vector3 CalculateCollisionForce()
    {
        // Example calculation: using relative velocity and mass
        // Note: You should adjust this logic based on your game's requirements

        Vector3 relativeVelocity = playerRb.velocity - enemyTransform.GetComponent<Rigidbody>().velocity;
        Debug.Log("relative velocity: " + relativeVelocity);

        float combinedMass = playerRb.mass + enemyTransform.GetComponent<Rigidbody>().mass;

        // The direction should be away from the player, normalized to get a direction vector
        Vector3 forceDirection = (enemyTransform.position - transform.position).normalized;
        
        // Calculate the force based on your game's physics rules
        float forceMagnitude = relativeVelocity.magnitude * combinedMass * retreatMultiplier;

        return forceDirection * forceMagnitude;
    }*/

    Vector3 CalculateCollisionForce()
    {
        // Calculate the relative velocity
        Vector3 relativeVelocity = playerRb.velocity - enemyTransform.GetComponent<Rigidbody>().velocity;
        
        // Check the absolute values of the x and y components of the relative velocity
        if (Mathf.Abs(relativeVelocity.x) > 0.7f || Mathf.Abs(relativeVelocity.y) > 0.5f)
        {
            // Return a zero vector if the condition is met
            return Vector3.zero;
        }
        Debug.Log("Relative velocity: " + relativeVelocity);

        // Continue with the original force calculation if the condition is not met
        float combinedMass = playerRb.mass + enemyTransform.GetComponent<Rigidbody>().mass;
        Vector3 forceDirection = (enemyTransform.position - transform.position).normalized;
        float forceMagnitude = relativeVelocity.magnitude * combinedMass * retreatMultiplier;

        return forceDirection * forceMagnitude;
    }



    private float CalculateDistanceToEnemy()
    {
        if (enemyTransform != null)
        {
            return Vector3.Distance(transform.position, enemyTransform.position);
        }
        return float.MaxValue; // Return a large number if the enemy is not assigned
    }
    void HandleAIMovement()
    {
        // Example AI logic (pseudocode)
        // Determine distance to player
        // If close enough, decide to attack
        // If far, move towards player
        // Randomly choose to defend at times
    }

    void EnsureGrounding()
    {
        RaycastHit hit;
        float maxDistanceToGround = 50.0f; // Maximum distance to check for the ground
        float groundingForce = 10.0f; // Force applied to ground the character

        if (!IsGrounded() && Physics.Raycast(transform.position, Vector3.down, out hit, maxDistanceToGround))
        {
            // Calculate the distance to the ground
            float distanceToGround = hit.distance;

            // If the character is too high above the ground, apply a downward force
            if (distanceToGround > 0.1f) // You can adjust this threshold
            {
                playerRb.AddForce(Vector3.down * groundingForce, ForceMode.Impulse);
            }
            else
            {
                // If the character is close to the ground, you can adjust the position directly
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }

        // Reset air time
        airTime = 0.0f;
    }

 
    void IncrementCombo()
    {
        comboTimer = comboMaxTime;
        attackIntensity++;
        if (attackIntensity > 3)
        {
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        attackIntensity = 1;
        comboTimer = 0;
    }
    void HandlePlayerMovement()
    {
        if (joystick == null) return;

        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Handling horizontal movement
        if (Mathf.Abs(horizontalInput) > walkThreshold)
        {
            // Player movement
            Vector3 moveDirection = new Vector3(0, 0, playerZOrientation*horizontalInput);
            transform.Translate(moveDirection * speed * Time.deltaTime);

            // Player animation
            playerAnimator.SetBool("inMotion", true);
            playerAnimator.SetFloat("joystickDragHorizontal", horizontalInput);

            // Differentiate between walking and running
            bool isRunning = Mathf.Abs(horizontalInput) > runThreshold;
            playerAnimator.SetBool("isRunning", isRunning);
        }
        else
        {
            // Resetting motion-related animations when idle
            playerAnimator.SetBool("inMotion", false);
            playerAnimator.SetFloat("joystickDragHorizontal", 0);
        }

        // Handling vertical movement (Jump and Crouch)
        if (verticalInput > walkThreshold)
        {
            // Jump
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }
        else if (verticalInput < -walkThreshold)
        {
            // Crouch
            playerAnimator.SetBool("isCrouching", true);
        }
        else
        {
            // Resetting vertical-related animations
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetBool("isCrouching", false);
        }
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
                playerZOrientation = 1;
            }
            else
            {
                playerZOrientation = -1;
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

    public void PerformLightAttack()
    {
        // Use the class-level attackIntensity variable
        //int attackIntensity = DetermineAttackIntensity(); // Remove this line

        int variation = Random.Range(0, 3); // For three variations

        ExecuteBeakAttackLevel(variation, attackIntensity);

        IncrementCombo();
    }

    void ExecuteBeakAttackLevel(int variation, int attackInt)
    {
        Debug.Log("Attack level: "+ attackIntensity+ " , Variation: " + variation);

        // Determine the specific movement based on variation
        switch (variation) //
        {
            case 0:
                PerformStandardBeakAttack(attackInt);
                break;
            case 1:
                PerformJumpingBeakAttack(attackInt);
                break;
            case 2:
                PerformUppercutBeakAttack(attackInt);
                break;
        }

        // Trigger corresponding animation for this variation
        // playerAnimator.SetTrigger("BeakAttack" + variation); this is to be called where finally deciding the attack based on intensity and variation
    }

    void PerformStandardBeakAttack(int attackInt)
    {
        float intensityOffset = 1.0f;

        if (attackInt == 2)
        {
            intensityOffset = 5.0f;
        }
        else if (attackInt == 3)
        {
            intensityOffset = 15.0f;
        }
        // Increase the forward force for a more pronounced forward attack
        Vector3 forwardForce = transform.forward * (15f + intensityOffset); // Forward force
        playerRb.AddForce(forwardForce, ForceMode.Impulse);

        // Apply a reduced upward force to prevent too much elevation
        Vector3 upwardForce = Vector3.up * 5f; // Reduced upward force
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

    }

    bool IsGrounded()
    {
        float groundCheckDistance = 0.1f; // Distance to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"); // Replace "Ground" with your ground layer name
        }
        return false;
    }


    void PerformJumpingBeakAttack(int attackInt)
    {
        float intensityOffset = 1.0f;

        if (attackInt == 2)
        {
            intensityOffset = 2.0f;
        }
        else if (attackInt == 3)
        {
            intensityOffset = 10.0f;
        }
        // Apply a stronger upward force for a pronounced jumping attack
        Vector3 upwardForce = Vector3.up * (5f + intensityOffset); // Increased upward force
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

        // Delay the forward force slightly to simulate the leap before lunging forward
        StartCoroutine(DelayedForwardAttack(0.001f, attackInt)); // Delay for 0.2 seconds
    }

    IEnumerator DelayedForwardAttack(float delay, int attackInt)
    {
        float intensityOffset = 1.0f;

        if (attackInt == 2)
        {
            intensityOffset = 5.0f;
        }
        else if (attackInt == 3)
        {
            intensityOffset = 15.0f;
        }
        yield return new WaitForSeconds(delay);

        // Forward force applied after the upward leap
        Vector3 forwardForce = transform.forward * (15f + intensityOffset); // Forward force similar to standard attack
        playerRb.AddForce(forwardForce, ForceMode.Impulse);
    }


    void PerformUppercutBeakAttack(int attackInt)
    {
        float intensityOffset = 1.0f;

        if (attackInt == 2)
        {
            intensityOffset = 5.0f;
        }
        else if (attackInt == 3)
        {
            intensityOffset = 15.0f;
        }
        // Apply an upward force to launch the player upwards, simulating the start of an uppercut
        Vector3 upwardForce = Vector3.up * (10f + intensityOffset); // Strong upward force for a pronounced lift
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

        // Delay the forward force to simulate the player preparing for the uppercut
        StartCoroutine(DelayedUppercutForwardAttack(0.05f, attackInt)); // Delay for 0.2 seconds to prepare for the forward strike
    }

    IEnumerator DelayedUppercutForwardAttack(float delay, int attackInt)
    {
        float intensityOffset = 1.0f;

        if (attackInt == 2)
        {
            intensityOffset = 5.0f;
        }
        else if (attackInt == 3)
        {
            intensityOffset = 15.0f;
        }
        yield return new WaitForSeconds(delay);

        // Apply a forward force at an upward angle to complete the uppercut motion
        Vector3 uppercutAngle = Quaternion.Euler(45, 0, 0) * transform.forward; // 45-degree angle upwards
        Vector3 uppercutForce = uppercutAngle * (15f + intensityOffset); // Forward force similar to standard attack but angled upward
        playerRb.AddForce(uppercutForce, ForceMode.Impulse);
    }


    // Similarly, implement ExecuteAttackLevel2 and ExecuteAttackLevel3
}
