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

    float speed = 5f;
    float attack_Distance = 3f;
    float chase_Player_After_Attack = 1f;
    float targetDist;
    float current_Attack_Time;
    float default_Attack_Time = 3f;

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
        current_Attack_Time = default_Attack_Time;
       
        enemyWeapons[0].gameObject.SetActive(false);
        enemyWeapons[1].gameObject.SetActive(false);
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
            EnemyAttack(Random.Range(0, 2));
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
        if (attack == 0 && enemyWeapons[1].gameObject.CompareTag("Right Arm"))
        {
            enemyWeapons[0].gameObject.SetActive(true);
            enemyAnimator.SetTrigger("isLightAttack");
        }

        else if(attack == 1 && enemyWeapons[0].gameObject.CompareTag("Left Arm"))
        {
            enemyWeapons[1].gameObject.SetActive(true);
            enemyAnimator.SetTrigger("isHeavyAttack");
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
}
