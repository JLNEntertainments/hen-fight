using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDecision : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    private PlayerGamePlayManager playerController; // Reference to the player's controller
    private float attackRange = 5f; // The range at which the enemy can initiate attacks
    private float lowHealthThreshold = 30f; // The health threshold below which the enemy considers itself low on health
    private float specialAttackThreshold = 0.5f; // The threshold probability for the enemy to detect a special attack from the player
    private float distanceToPlayer; // The current distance to the player
    private float currentHealth; // The current health of the enemy

    // Other properties may include variables related to movement speed, animation references, etc.

    private void Start()
    {
        // Initialization logic, such as finding the player object and setting up initial values
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerGamePlayManager>();
        // Additional initialization code...
    }

    private void Update()
    {
        // Update logic that needs to happen every frame
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // Additional update code...
        MakeDecision();
    }

    private void MakeDecision()
    {
        // The decision-making logic based on the player's state, health, and other factors
        if (IsPlayerInRange())
        {
            if (IsLowOnHealth())
            {
                Defend();
            }
            else if (IsPlayerPerformingSpecialAttack())
            {
                Dodge();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    // Other methods may include functions for handling specific actions, animations, etc.

    private bool IsPlayerInRange()
    {
        // Logic to check if the player is within attack range
        return distanceToPlayer < attackRange;
    }

    private bool IsLowOnHealth()
    {
        // Logic to check if the enemy is low on health
        return currentHealth < lowHealthThreshold;
    }

    private bool IsPlayerPerformingSpecialAttack()
    {
        // Logic to check if the player is performing a special attack (this is a placeholder)
        return Random.value < specialAttackThreshold;
    }

    private void Attack()
    {
        // Logic for the attack action
    }

    private void Defend()
    {
        // Logic for the defend action
    }

    private void Attack2()
    {
        // Logic for the attack2 action
    }

     private void Dodge()
    {
        // Logic for the dodge action
    }

    private void MoveTowardsPlayer()
    {
        // Logic to move towards the player
    }
}
