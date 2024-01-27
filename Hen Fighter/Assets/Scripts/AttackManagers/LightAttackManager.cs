using UnityEngine;
using System.Collections;

public class LightAttackManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody playerRb;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Transform playerTransform;

    public void Initialize(Rigidbody rb, Animator animator, Transform transform)
    {
        playerRb = rb;
        playerAnimator = animator;
        playerTransform = transform;
    }

    public void PerformLightAttack(int attackIntensity, int variation)
    {
        // Use the class-level attackIntensity variable
        switch (attackIntensity)
        {
            case 1:
                ExecuteAttackLevel1(variation);
                break;
            case 2:
                ExecuteAttackLevel2(variation);
                break;
            case 3:
                ExecuteAttackLevel3(variation);
                break;
        }
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

