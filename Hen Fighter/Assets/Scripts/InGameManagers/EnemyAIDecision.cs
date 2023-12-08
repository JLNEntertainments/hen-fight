using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDecision : MonoBehaviour
{
    EnemyGamePlayManager enemyGamePlayManager;

    float lowHealthThreshold = 0.2f;
    float lowStaminaThreshold = 0.3f;
    float specialAttackThreshold = 0.5f;
    float distanceToPlayer;

    private void Start()
    {
        enemyGamePlayManager = this.GetComponent<EnemyGamePlayManager>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(this.transform.position, enemyGamePlayManager.playerGamePlayManager.transform.position);

        enemyGamePlayManager.current_Attack_Time += Time.deltaTime;
        enemyGamePlayManager.enemy_Start += Time.deltaTime;

        if (enemyGamePlayManager.enemy_Start > 2.5f)
            MakeMovementDecision();
    }

    void FixedUpdate()
    {
        if ((enemyGamePlayManager.current_Attack_Time > enemyGamePlayManager.default_Attack_Time) && !enemyGamePlayManager.isTakingDamage /*&& (enemyGamePlayManager.enemy_Stamina > 0)*/)
        {
            MakeCombatDecision();
            enemyGamePlayManager.current_Attack_Time = 0;
        }
    }

    private void MakeCombatDecision()
    {
        //For making decisions when player is in attack range
        if (IsPlayerInAttackRange() && !IsEnemyLowOnHealth())
        {
            enemyGamePlayManager.Attack();

            /*if (IsPlayerPerformingSpecialAttack())
                Dodge();*/
        }

        if (IsEnemyLowOnHealth())
        {
            Defend();
        }
    }

    void MakeMovementDecision()
    {
        //For making decisions when player is not in attack range
        if (IsPlayerInChaseRange() && !IsPlayerLowOnStamina() && !IsEnemyLowOnHealth())
        {
            enemyGamePlayManager.FollowTarget();
        }
        else if (!IsPlayerLowOnStamina())
        {
            enemyGamePlayManager.PrepareAttack();
        }
        /*else if (IsPlayerLowOnStamina())
        {
            Debug.Log("----Moving away");
            //move away from player
        }*/
    }

    public bool IsPlayerInAttackRange()
    {
        return distanceToPlayer <= enemyGamePlayManager.attack_Distance ;
    }

    public bool IsPlayerInChaseRange()
    {
        return distanceToPlayer > enemyGamePlayManager.attack_Distance;
    }

    bool IsPlayerLowOnHealth()
    {
        return enemyGamePlayManager.playerGamePlayManager.playerHealth < lowHealthThreshold;
    }

    bool IsEnemyLowOnHealth()
    {
        return enemyGamePlayManager.enemyHealth < lowHealthThreshold;
    }

    bool IsPlayerLowOnStamina()
    {
        return ScoreManager.Instance.characterStaminaValuePlayer < lowStaminaThreshold;
    }

    bool IsPlayerPerformingSpecialAttack()
    {
        return Random.value < specialAttackThreshold;
    }

    private void Attack()
    {
        // Logic for the attack action
    }

    private void Defend()
    {
        Debug.Log("----Defending");
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
