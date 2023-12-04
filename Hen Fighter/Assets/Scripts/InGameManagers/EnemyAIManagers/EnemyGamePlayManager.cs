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

    StaminaHandlerManager enemyStaminaHandler;

    Image healthBar;

    float speed, attack_Distance, chase_Player_After_Attack, targetDist, current_Attack_Time, default_Attack_Time;

    [HideInInspector]
    public float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    [HideInInspector]
    public bool followPlayer, attackPlayer, isHeavyAttack, isLightAttack;

    public DamageGeneric[] enemyWeapons;

    [SerializeField]
    string currentAnimaton;

    //Animation States
    const string ENEMY_IDLE = "Idle";
    const string ENEMY_WALK = "Walk";
    const string ENEMY_BACKWALK = "BackWalk";
    const string ENEMY_LIGHTATTACK = "BeakAttack";
    const string ENEMY_HEAVYATTACK = "ClawAttack";
    const string ENEMY_BLOCK = "Block";
    const string ENEMY_HURT = "React";

    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody>();

        playerTarget = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        enemyWeapons = GetComponentsInChildren<DamageGeneric>();
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();
        enemyStaminaHandler = GetComponent<StaminaHandlerManager>();
        
        speed = 1f;
        attack_Distance = 3f;
        chase_Player_After_Attack = 1f;
        default_Attack_Time = 4f;
        default_Stamina_Regen_Time = 8f;

        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;

        TurnOffAttackpoints();
    }

    void Update()
    {
        //Attack();
        if (!attackPlayer && !followPlayer)
            StaminaRegeneration();
    }

    void FixedUpdate()
    {
        FollowTarget();
        UpdateEnemyRotation();
        Attack();
    }

    void FollowTarget()
    {
        targetDist = Vector3.Distance(transform.position, playerTarget.position);

        if (targetDist > attack_Distance)
        {
            transform.LookAt(playerTarget.transform);
            myBody.velocity = transform.forward * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                ChangeAnimationState(ENEMY_WALK);
                //enemyAnimator.SetBool("inMotion", true);
                followPlayer = true;
            }
        }
        else if (targetDist <= attack_Distance)
        {
            myBody.velocity = Vector3.zero;
            ChangeAnimationState(ENEMY_IDLE);
            //enemyAnimator.SetBool("inMotion", false);
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
            //StartCoroutine(EnemyAttack()); //0 for LightAttack, 1 for HeavyAttack
            //StopCoroutine(EnemyAttack());
            EnemyAttack(Random.Range(0, 2));
            current_Attack_Time = 0f;
            attackPlayer = true;
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
            if (attack == 1 && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                //enemyAnimator.SetTrigger("isLightAttack");
                ChangeAnimationState(ENEMY_LIGHTATTACK);
                isLightAttack = true;
                isHeavyAttack = false;
                ScoreManager.Instance.UpdateEnemyScore("isLight");
                //yield return new WaitForSeconds(1f);
            }

            else if (attack == 0 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                //enemyAnimator.SetTrigger("isHeavyAttack");
                ChangeAnimationState(ENEMY_HEAVYATTACK);
                isHeavyAttack = true;
                isLightAttack = false;
                ScoreManager.Instance.UpdateEnemyScore("isHeavy");
                //yield return new WaitForSeconds(1f);
                //this.transform.position = new Vector3(this.transform.position.x - 1.85f, this.transform.position.y, this.transform.position.z);
                
            }
        } 
    }

    void StaminaRegeneration()
    {
        current_Stamina_Regen_Time += Time.deltaTime;
        if (current_Stamina_Regen_Time >= default_Stamina_Regen_Time && enemyStaminaHandler.characterStamina < enemyStaminaHandler.maxStamina)
        {
            enemyStaminaHandler.IncreaseStamina();
            current_Stamina_Regen_Time = 0;
        }
    }

    void UpdateEnemyRotation()
    {
        transform.eulerAngles = new Vector3(0, -90f, 0);
    }

    public void InflictEnemyDamage()
    {
        //enemyAnimator.SetTrigger("isHurt");
        ChangeAnimationState(ENEMY_HURT);
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

        enemyAnimator.PlayInFixedTime(newAnimation);
        currentAnimaton = newAnimation;
    }
}
