using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDecision : MonoBehaviour
{
    EnemyGamePlayManager enemyGamePlayManager;

    float lowHealthThreshold = 0f;
    float lowStaminaThreshold = 0.3f;
    float distanceToPlayer;
    float defendAttackRandom;

    private void Start()
    {
        enemyGamePlayManager = this.GetComponent<EnemyGamePlayManager>();
        defendAttackRandom = 5f;
    }

    void Update()
    {
        if(enemyGamePlayManager.isPlayerFound)
            distanceToPlayer = Vector3.Distance(this.transform.position, enemyGamePlayManager.playerGamePlayManager.transform.position);

        defendAttackRandom = Random.Range(0, 5f);

        enemyGamePlayManager.current_Attack_Time += Time.deltaTime;
        enemyGamePlayManager.enemy_Start += Time.deltaTime;
        enemyGamePlayManager.block_Attack_Time += Time.deltaTime;

        if (enemyGamePlayManager.enemy_Start > 5.5f)
            MakeMovementDecision();
    }

    private void FixedUpdate()
    {
        if ((enemyGamePlayManager.current_Attack_Time > enemyGamePlayManager.default_Attack_Time))
        {
            enemyGamePlayManager.TurnOffAttackpoints();
            MakeCombatDecision();
            enemyGamePlayManager.current_Attack_Time = 0;
        }
    }

    private void MakeCombatDecision()
    {
        //For making decisions when player is in attack range
        if (!IsEnemyLowOnHealth())
        {
            enemyGamePlayManager.Attack();
        }
        /*else
        {
            if (enemyGamePlayManager.block_Attack_Time > defendAttackRandom)
            {
                Defend();
                enemyGamePlayManager.block_Attack_Time = 0;
            }
        }*/
    }

    void MakeMovementDecision()
    {
        //For making decisions when player is not in attack range
        if(!IsPlayerPerformingSpecialAttack())
        {
            if (IsPlayerInChaseRange() && !IsPlayerLowOnStamina() && !IsEnemyLowOnHealth())
                enemyGamePlayManager.FollowTarget();
            else if (!IsPlayerLowOnStamina())
                enemyGamePlayManager.PrepareAttack();
            /*else if (IsPlayerLowOnStamina())
                enemyGamePlayManager.UnFollowTarget();*/
        }
        else
        {
            enemyGamePlayManager.SpecialAttackPlaying();
            enemyGamePlayManager.playerGamePlayManager.isSpecialAttack = false;
        }
    }

    public bool IsPlayerInAttackRange()
    {
        return distanceToPlayer <= enemyGamePlayManager.attack_Distance;
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
        //return ScoreManager.Instance.enemyHealth < lowHealthThreshold;
        return false;
    }

    bool IsPlayerLowOnStamina()
    {
        return ScoreManager.Instance.characterStaminaValuePlayer < lowStaminaThreshold;
    }

    bool IsPlayerPerformingSpecialAttack()
    {
        return enemyGamePlayManager.playerGamePlayManager.isSpecialAttack;
    }

    private void Attack()
    {
        // Logic for the attack action
    }

    private void Defend()
    {
        enemyGamePlayManager.Defend();
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
