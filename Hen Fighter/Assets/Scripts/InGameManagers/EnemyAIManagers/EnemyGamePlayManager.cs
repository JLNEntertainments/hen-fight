using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;

    EnemyAIDecision enemyAIDecision;

    Animator enemyAnimator;

    Rigidbody myBody;

    Image healthBar;

    [HideInInspector]
    public float enemyHealth;

    float speed;

    [HideInInspector]
    public float default_Attack_Time, current_Attack_Time, enemy_Start, enemy_Stamina;

    [HideInInspector]
    public float attack_Distance;
    
    [HideInInspector]
    public float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    [HideInInspector]
    public bool followPlayer, attackPlayer, isHeavyAttack, isLightAttack, isTakingDamage, isAttacking;

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
        enemyAIDecision = GetComponent<EnemyAIDecision>();

        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
    }

    void Start()
    {
        enemyWeapons = GetComponentsInChildren<DamageGeneric>();
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();
        
        speed = 2f;
        enemyHealth = 1f;
        attack_Distance = 2.5f;
        enemy_Stamina = ScoreManager.Instance.characterStaminaValueEnemy;

        default_Attack_Time = 3f;
        default_Stamina_Regen_Time = 8f;
        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;
        enemy_Start = 0;

        TurnOffAttackpoints();
    }

    void Update()
    {
        /*current_Attack_Time += Time.deltaTime;
        enemy_Start += Time.deltaTime;*/

        UpdateEnemyRotation();

        /*if (enemy_Start > 4f)
            FollowTarget();*/
    }

    void FixedUpdate()
    {
        /*if ((current_Attack_Time > default_Attack_Time) && !isTakingDamage && (enemy_Stamina > 0))
        {
            Attack();
            current_Attack_Time = 0;
        }*/
    }

    public void FollowTarget()
    {
        if (enemyAIDecision.IsPlayerInChaseRange())
        {
            transform.LookAt(playerGamePlayManager.transform);
            myBody.velocity = Vector3.left * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                ChangeAnimationState(ENEMY_WALK);
                followPlayer = true;
            }
        }
    }

    public void UnFollowTarget()
    {
        //myBody.velocity = Vector3.right * speed;

        /*if (myBody.velocity.sqrMagnitude != 0)
        {*/
            ChangeAnimationState(ENEMY_BACKWALK);
            followPlayer = false;
        //}
    }

    public void PrepareAttack()
    {
        myBody.velocity = Vector3.zero;
        ChangeAnimationState(ENEMY_IDLE);
        followPlayer = false;
        attackPlayer = true;
    }

    public void Attack()
    {
        if (!attackPlayer)
            return;

        StartCoroutine(EnemyAttack());
        
        if (!enemyAIDecision.IsPlayerInAttackRange())
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
                SetDefaultAnimationState();
                obj.gameObject.SetActive(false);
            }

            if (attack == 0 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                ChangeAnimationState(ENEMY_HEAVYATTACK);
                isHeavyAttack = true;
                isLightAttack = false;
                yield return new WaitForSeconds(1f);
                SetDefaultAnimationState();
                obj.gameObject.SetActive(false);
                //this.transform.position = new Vector3(this.transform.position.x - 1.85f, this.transform.position.y, this.transform.position.z);
            }
        } 
    }

    void UpdateEnemyRotation()
    {
        transform.eulerAngles = new Vector3(0, -90f, 0);
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

    public void SetDefaultAnimationState()
    {
        enemyAnimator.Play(ENEMY_IDLE);
        currentAnimaton = ENEMY_IDLE;
    }

    public void InflictEnemyDamage()
    {
        isTakingDamage = true;
        StartCoroutine(PlayHurtAnimation());
        enemyHealth -= 0.1f;
        healthBar.fillAmount = enemyHealth;
    }

    IEnumerator PlayHurtAnimation()
    {
        ChangeAnimationState(ENEMY_HURT);
        yield return new WaitForSeconds(2f);
        SetDefaultAnimationState();
        isTakingDamage = false;
    }
}
