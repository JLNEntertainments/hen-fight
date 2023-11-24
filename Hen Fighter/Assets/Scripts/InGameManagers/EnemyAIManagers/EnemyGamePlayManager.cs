using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : SingletonGeneric<EnemyGamePlayManager>
{
    Animator enemyAnimator;

    Rigidbody myBody;
    
    Transform playerTarget;

    Image healthBar;

    float speed, attack_Distance, chase_Player_After_Attack, targetDist, current_Attack_Time, default_Attack_Time;

    bool followPlayer, attackPlayer;

    public DamageGeneric[] enemyWeapons;

    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody>();

        playerTarget = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        enemyWeapons = gameObject.GetComponentsInChildren<DamageGeneric>();
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();

        followPlayer = true;
        
        speed = 3f;
        attack_Distance = 2.5f;
        chase_Player_After_Attack = 1f;
        default_Attack_Time = 3f;

        current_Attack_Time = default_Attack_Time;

        TurnOffAttackpoints();
    }

    void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
       FollowTarget();
       UpdateEnemyRotation();
    }

    void FollowTarget()
    {
        if (!followPlayer)
            return;

        targetDist = Vector3.Distance(transform.position, playerTarget.position);

        if (targetDist > attack_Distance)
        {
            transform.LookAt(playerTarget.transform);
            myBody.velocity = transform.forward * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                enemyAnimator.SetBool("inMotion", true);
            }

        }
        else if (Vector3.Distance(transform.position, playerTarget.position) <= attack_Distance)
        {
            myBody.velocity = Vector3.zero;
            enemyAnimator.SetBool("inMotion", false);

            followPlayer = false;
            attackPlayer = true;
        }
    }

    void Attack()
    {
        if (!attackPlayer)
            return;

        current_Attack_Time += Time.deltaTime;

        if (current_Attack_Time > default_Attack_Time)
        {
            EnemyAttack(Random.Range(0, 2)); //0 for LightAttack, 1 for HeavyAttack
            current_Attack_Time = 0f;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) > attack_Distance + chase_Player_After_Attack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    void EnemyAttack(int attack)
    {
        foreach(var obj in enemyWeapons) 
        {
            if (attack == 0 && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isLightAttack");
                return;
            }

            else if (attack == 1 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isHeavyAttack");
                return;
            }
        } 
    }

    void UpdateEnemyRotation()
    {
        transform.eulerAngles = new Vector3(0, 90f, 0);
    }

    public void InflictEnemyDamage()
    {
        enemyAnimator.SetTrigger("isHurt");
        healthBar.fillAmount -= 0.1f;
    }

    void TurnOffAttackpoints()
    {
        foreach(var obj in enemyWeapons)
            obj.gameObject.SetActive(false);
    }
}
