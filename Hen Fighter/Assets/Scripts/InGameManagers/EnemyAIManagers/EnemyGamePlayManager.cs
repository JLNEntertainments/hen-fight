using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;

    Animator enemyAnimator;

    Rigidbody myBody;

    StaminaHandlerManager enemyStaminaHandler;

    Image healthBar;

    float speed, attack_Distance, chase_Player_After_Attack, targetDist, current_Attack_Time, default_Attack_Time, enemy_Start;

    [HideInInspector]
    public float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    [HideInInspector]
    public bool followPlayer, attackPlayer, isHeavyAttack, isLightAttack, isTakingDamage;

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

        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
    }

    void Start()
    {
        enemyWeapons = GetComponentsInChildren<DamageGeneric>();
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();
        enemyStaminaHandler = GetComponent<StaminaHandlerManager>();
        
        speed = 2f;
        attack_Distance = 3f;
        chase_Player_After_Attack = 1f;
        default_Attack_Time = 4f;
        default_Stamina_Regen_Time = 8f;

        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;
        enemy_Start = 0;

        TurnOffAttackpoints();
        //InvokeRepeating("Attack", 5f, default_Attack_Time);
        //InvokeRepeating("FollowTarget", 1f, 0.1f);
    }

    void Update()
    {
        current_Attack_Time += Time.deltaTime;
        enemy_Start += Time.deltaTime;

        UpdateEnemyRotation();
        if (enemy_Start > 4f)
            FollowTarget();
    }

    void FixedUpdate()
    {
        if (current_Attack_Time > default_Attack_Time && !isTakingDamage)
        {
            Attack();
            current_Attack_Time = 0;
        }
    }

    void FollowTarget()
    {
        targetDist = Vector3.Distance(transform.position, playerGamePlayManager.transform.position);

        if (targetDist > attack_Distance)
        {
            transform.LookAt(playerGamePlayManager.transform);
            myBody.velocity = transform.forward * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                ChangeAnimationState(ENEMY_WALK);
                followPlayer = true;
            }
        }
        
        if (targetDist <= attack_Distance)
        {
            myBody.velocity = Vector3.zero;
            ChangeAnimationState(ENEMY_IDLE);
            followPlayer = false;
            attackPlayer = true;
        }
    }

    void Attack()
    {
        if (!attackPlayer)
            return;

        StartCoroutine(EnemyAttack());

        if (Vector3.Distance(transform.position, playerGamePlayManager.transform.position) < attack_Distance + chase_Player_After_Attack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    IEnumerator EnemyAttack()
    {
        int attack = (Random.Range(0, 2));

        foreach (var obj in enemyWeapons) 
        {
            if (attack == 1 && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                ChangeAnimationState(ENEMY_LIGHTATTACK);
                isLightAttack = true;
                isHeavyAttack = false;
                yield return new WaitForSeconds(0.5f);
                ResetAnimationState();
                obj.gameObject.SetActive(false);
            }

            if (attack == 0 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                ChangeAnimationState(ENEMY_HEAVYATTACK);
                isHeavyAttack = true;
                isLightAttack = false;
                yield return new WaitForSeconds(1f);
                ResetAnimationState();
                obj.gameObject.SetActive(false);
                //this.transform.position = new Vector3(this.transform.position.x - 1.85f, this.transform.position.y, this.transform.position.z);

            }
        } 
    }

    void UpdateEnemyRotation()
    {
        transform.eulerAngles = new Vector3(0, -90f, 0);
    }

    public void InflictEnemyDamage()
    {
        isTakingDamage = true;
        //ChangeAnimationState(ENEMY_HURT);
        StartCoroutine(PlayHurtAnimation());
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

    public void ResetAnimationState()
    {
        enemyAnimator.Play(ENEMY_IDLE);
        currentAnimaton = ENEMY_IDLE;
    }

    IEnumerator PlayHurtAnimation()
    {
        //ChangeAnimationState(ENEMY_HURT);
        enemyAnimator.SetTrigger("isHurt");
        yield return new WaitForSeconds(2f);
        ResetAnimationState();
        isTakingDamage = false;
    }
}
