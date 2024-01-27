using System.Collections;
using UnityEngine;

public class PlayerGamePlayManager : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    private Animator playerAnimator;
    private Rigidbody playerRb;

    [SerializeField]
    private float speed = 2f;
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

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        RotateTowardsEnemy();

        
        // Combo timing logic
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
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
    void HandleMovement()
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
        // int attackIntensity = DetermineAttackIntensity(); // Remove this line

        int variation = Random.Range(0, 3); // For three variations

        switch (attackIntensity)
        {
            case 1:
                ExecuteAttackLevel1(variation);
                break;
            case 2:
                ExecuteAttackLevel1(variation); // ExecuteAttackLevel2(variation);
                break;
            case 3:
                ExecuteAttackLevel1(variation); // ExecuteAttackLevel3(variation);
                break;
        }

        IncrementCombo();
    }

    void ExecuteAttackLevel1(int variation)
    {
        Debug.Log("Attack level: 1, Variation: " + variation);

        // Determine the specific movement based on variation
        switch (variation) //
        {
            case 0:
                PerformStandardBeakAttack();
                break;
            case 1:
                PerformJumpingBeakAttack();
                break;
            case 2:
                PerformUppercutBeakAttack();
                break;
        }

        // Trigger corresponding animation for this variation
        playerAnimator.SetTrigger("BeakAttack" + variation);
    }

    void PerformStandardBeakAttack()
    {
        // Increase the forward force for a more pronounced forward attack
        Vector3 forwardForce = transform.forward * 15f; // Forward force
        playerRb.AddForce(forwardForce, ForceMode.Impulse);

        // Apply a reduced upward force to prevent too much elevation
        Vector3 upwardForce = Vector3.up * 5f; // Reduced upward force
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

       
    }


    void PerformJumpingBeakAttack()
    {
        // Apply a stronger upward force for a pronounced jumping attack
        Vector3 upwardForce = Vector3.up * 5f; // Increased upward force
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

        // Delay the forward force slightly to simulate the leap before lunging forward
        StartCoroutine(DelayedForwardAttack(0.001f)); // Delay for 0.2 seconds
    }

    IEnumerator DelayedForwardAttack(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Forward force applied after the upward leap
        Vector3 forwardForce = transform.forward * 15f; // Forward force similar to standard attack
        playerRb.AddForce(forwardForce, ForceMode.Impulse);
    }


    void PerformUppercutBeakAttack()
    {
        // Apply an upward force to launch the player upwards, simulating the start of an uppercut
        Vector3 upwardForce = Vector3.up * 10f; // Strong upward force for a pronounced lift
        playerRb.AddForce(upwardForce, ForceMode.Impulse);

        // Delay the forward force to simulate the player preparing for the uppercut
        StartCoroutine(DelayedUppercutForwardAttack(0.05f)); // Delay for 0.2 seconds to prepare for the forward strike
    }

    IEnumerator DelayedUppercutForwardAttack(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Apply a forward force at an upward angle to complete the uppercut motion
        Vector3 uppercutAngle = Quaternion.Euler(45, 0, 0) * transform.forward; // 45-degree angle upwards
        Vector3 uppercutForce = uppercutAngle * 15f; // Forward force similar to standard attack but angled upward
        playerRb.AddForce(uppercutForce, ForceMode.Impulse);
    }



    void ExecuteAttackLevel2(int variation)
    {
        Debug.Log("Attack level: 2");
        // Apply movement and physics for level 1 attack with the specified variation
        // Trigger corresponding animation
    }

    void ExecuteAttackLevel3(int variation)
    {
        Debug.Log("Attack level: 3");
        // Apply movement and physics for level 1 attack with the specified variation
        // Trigger corresponding animation
    }

    // Similarly, implement ExecuteAttackLevel2 and ExecuteAttackLevel3
}
