using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIDecision : MonoBehaviour
{
    EnemyGamePlayManager enemyGamePlayManager;

    float lowHealthThreshold = 0f;
    float lowStaminaThreshold = 0.15f;
    float distanceToPlayer;
    float defendAttackRandom;
    int random;
    public bool backWalkToggle;

    private void Start()
    {
        enemyGamePlayManager = this.GetComponent<EnemyGamePlayManager>();
        defendAttackRandom = 5f;

        InvokeRepeating("GetRandomIndex", 7f, 4f);
        //InvokeRepeating("SetBackWalkToggle", 3f, 5f);
    }

    void Update()
    {
        if (enemyGamePlayManager.isPlayerFound)
            distanceToPlayer = Vector3.Distance(this.transform.position, enemyGamePlayManager.playerGamePlayManager.transform.position);

        defendAttackRandom = Random.Range(0, 5f);

        enemyGamePlayManager.current_Attack_Time += Time.deltaTime;
        enemyGamePlayManager.enemy_Start += Time.deltaTime;
        enemyGamePlayManager.block_Attack_Time += Time.deltaTime;

        if (enemyGamePlayManager.enemy_Start > 5.5f) 
        {
            MakeMovementDecision();
            enemyGamePlayManager.enemy_Unfollow_Time += Time.deltaTime;
        }  
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

    void GetRandomIndex()
    {
        random = Random.Range(0, 2);
    }

    void SetBackWalkToggle()
    {
        backWalkToggle = true;
    }

    private void MakeCombatDecision()
    {
        //For making decisions when player is in attack range
        if (!IsEnemyLowOnHealth() && enemyGamePlayManager.current_Attack_Time > enemyGamePlayManager.default_Attack_Time && !enemyGamePlayManager.unfollowTarget)
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
            Debug.Log("------" + enemyGamePlayManager.enemy_Unfollow_Time);
            if (!enemyGamePlayManager.unfollowTarget && random == 0)
            {
                if (IsPlayerInChaseRange() && !IsEnemyLowOnHealth())
                    enemyGamePlayManager.FollowTarget();
                else if (!IsEnemyLowOnStamina())
                    enemyGamePlayManager.PrepareAttack();
            }
            else if(random == 1 && enemyGamePlayManager.enemy_Unfollow_Time > 10f)
            {
                enemyGamePlayManager.UnFollowTarget();
                enemyGamePlayManager.unfollowTarget = false;
            }

        }
        /*else if (enemyGamePlayManager.enemy_Unfollow_Time > 6f)
        {
            enemyGamePlayManager.UnFollowTarget();
            enemyGamePlayManager.enemy_Unfollow_Time = 0;
        }*/

        else
        {
            enemyGamePlayManager.PlayAnimation("SpecialReact");
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
        return ScoreManager.Instance.enemyHealth < lowHealthThreshold;
        //return false;
    }

    bool IsEnemyLowOnStamina()
    {
        return ScoreManager.Instance.characterStaminaValueEnemy < lowStaminaThreshold;
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
