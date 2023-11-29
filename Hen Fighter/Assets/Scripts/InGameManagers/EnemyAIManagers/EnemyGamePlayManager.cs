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

    string currentAnimaton;

    //Animation States
    const string ENEMY_IDLE = "Idle";
    const string ENEMY_WALK = "Walking";
    const string ENEMY_BACKWALK = "BackWalk";
    const string ENEMY_LIGHTATTACK = "LightAttack";
    const string ENEMY_HEAVYATTACK = "HeavyAttack";
    const string ENEMY_BLOCK = "Block";
    const string ENEMY_HURT = "Hurt";

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
        
        speed = 1f;
        attack_Distance = 2.5f;
        chase_Player_After_Attack = 1f;
        default_Attack_Time = 2f;

        current_Attack_Time = default_Attack_Time;

        TurnOffAttackpoints();
    }

    void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
       //FollowTarget();
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
                //ChangeAnimationState(ENEMY_WALK);
                enemyAnimator.SetBool("inMotion", true);
            }
        }
        else if (targetDist <= attack_Distance)
        {
            myBody.velocity = Vector3.zero;
            //ChangeAnimationState(ENEMY_IDLE);
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
            /*StartCoroutine(EnemyAttack()); //0 for LightAttack, 1 for HeavyAttack
            StopCoroutine(EnemyAttack());*/
            EnemyAttack(Random.Range(0, 2));
            current_Attack_Time = 0f;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) < attack_Distance + chase_Player_After_Attack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    void EnemyAttack(int attack)
    {
        //int attack = (Random.Range(0, 2));

        foreach (var obj in enemyWeapons) 
        {
            if (attack == 0 && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isLightAttack");
                //ChangeAnimationState(ENEMY_LIGHTATTACK);
                
                //yield return new WaitForSeconds(2f);
            }

            else if (attack == 1 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isHeavyAttack");
                //ChangeAnimationState(ENEMY_HEAVYATTACK);
                
                //yield return new WaitForSeconds(2f);
            }
        } 
    }

    void UpdateEnemyRotation()
    {
        //transform.eulerAngles = new Vector3(0, 90f, 0);
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

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        enemyAnimator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
