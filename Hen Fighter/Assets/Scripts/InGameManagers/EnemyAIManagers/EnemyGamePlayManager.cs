using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;

    [HideInInspector]
    public EnemyAIDecision enemyAIDecision;

    Animator enemyAnimator;

    Rigidbody myBody;

    Image healthBar;

    float speed, lightAttackBuffer, heavyAttackBuffer, blockAttackBuffer, lightReactBuffer, heavyReactBuffer, specialReactBuffer;
    WaitForSeconds lightBuffer, heavyBuffer, blockBuffer, lReactBuffer, hReactBuffer, sReactBuffer;

    [HideInInspector]
    public float enemyHealth;

    [HideInInspector]
    public float default_Attack_Time, current_Attack_Time, enemy_Start, enemy_Stamina, block_Attack_Time;

    [HideInInspector]
    public float attack_Distance;

    [HideInInspector]
    public float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    [HideInInspector]
    public bool followPlayer, attackPlayer, isHeavyAttack, isLightAttack, isTakingDamage, isAttacking, isBlocking, isPlayerFound, isPlayingAnotherAnimation;

    public DamageGeneric[] enemyWeapons;

    [SerializeField]
    string currentAnimaton;

    //Animation States
    string ENEMY_IDLE, ENEMY_WALK, ENEMY_BACKWALK, ENEMY_LIGHTATTACK, ENEMY_HEAVYATTACK, ENEMY_BLOCK, ENEMY_LIGHTREACT, ENEMY_HEAVYREACT, ENEMY_SPECIALREACT;

    private AudioSource EnemeyAudio;

    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody>();
        enemyAIDecision = GetComponent<EnemyAIDecision>();
    }

    void Start()
    {

        enemyWeapons = GetComponentsInChildren<DamageGeneric>();
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();

        speed = 2f;
        enemyHealth = 1f;
        attack_Distance = 2.5f;
        lightAttackBuffer = 0.5f;
        heavyAttackBuffer = 1f;
        blockAttackBuffer = 1.2f;
        lightReactBuffer = 0.8f;
        heavyReactBuffer = 1.5f;
        specialReactBuffer = 1f;

        lightBuffer = new WaitForSeconds(lightAttackBuffer);
        heavyBuffer = new WaitForSeconds(heavyAttackBuffer);
        blockBuffer = new WaitForSeconds(blockAttackBuffer);
        lReactBuffer = new WaitForSeconds(lightReactBuffer);
        hReactBuffer = new WaitForSeconds(heavyReactBuffer);
        sReactBuffer = new WaitForSeconds(specialReactBuffer);

        enemy_Stamina = ScoreManager.Instance.characterStaminaValueEnemy;

        default_Attack_Time = 3f;
        default_Stamina_Regen_Time = 8f;
        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;
        enemy_Start = 0;

        EnemeyAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        
        ENEMY_IDLE = "Idle";
        ENEMY_WALK = "Walking";
        ENEMY_BACKWALK = "BackWalk";
        ENEMY_LIGHTATTACK = "LightAttack";
        ENEMY_HEAVYATTACK = "HeavyAttack";
        ENEMY_BLOCK = "Crouch";
        ENEMY_LIGHTREACT = "LightReact";
        ENEMY_HEAVYREACT = "HeavyReact";
        ENEMY_SPECIALREACT = "SpecialReact";

        TurnOffAttackpoints();
    }

    void Update()
    {
        if (!isPlayerFound && FindObjectOfType<PlayerGamePlayManager>())
        {
            playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
            isPlayerFound = true;
            PlayerCombatManager.Instance.AssignplayerAttributes();
        }

        UpdateEnemyRotation();
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
        ChangeAnimationState(ENEMY_BACKWALK);
        followPlayer = false;
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

        if (!playerGamePlayManager.canPerformCombat)
        {
            playerGamePlayManager.canPerformCombat = true;
            StartCoroutine(EnemyAttack());
            StopCoroutine(EnemyAttack());
            playerGamePlayManager.canPerformCombat = false;
        }

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
            if (attack == 1 && obj.gameObject.CompareTag("Beak") && !isPlayingAnotherAnimation)
            {
                isPlayingAnotherAnimation = true;
                obj.gameObject.SetActive(true);
                ChangeAnimationState(ENEMY_LIGHTATTACK);
                EnemeyAudio.Play();
                isLightAttack = true;
                isHeavyAttack = false;
                yield return lightBuffer;
                SetDefaultAnimationState();
                obj.gameObject.SetActive(false);
                isPlayingAnotherAnimation = false;
            }

            if (attack == 0 && obj.gameObject.CompareTag("Foot") && !isPlayingAnotherAnimation)
            {
                isPlayingAnotherAnimation = true;
                obj.gameObject.SetActive(true);
                ChangeAnimationState(ENEMY_HEAVYATTACK);
                EnemeyAudio.Play();
                isHeavyAttack = true;
                isLightAttack = false;
                yield return heavyBuffer;
                SetDefaultAnimationState();
                obj.gameObject.SetActive(false);
                isPlayingAnotherAnimation = false;
                //this.transform.position = new Vector3(this.transform.position.x - 1.85f, this.transform.position.y, this.transform.position.z);
            }
        }
    }

    public void Defend()
    {
        isBlocking = true;
        StartCoroutine(DefendAttack());
        StopCoroutine(DefendAttack());

    }

    IEnumerator DefendAttack()
    {
        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            ChangeAnimationState(ENEMY_BLOCK);
            yield return blockBuffer;
            SetDefaultAnimationState();
            isBlocking = false;
            isPlayingAnotherAnimation = false;
        }
    }

    void UpdateEnemyRotation()
    {
        transform.eulerAngles = new Vector3(0, -90f, 0);
    }

    void TurnOffAttackpoints()
    {
        foreach (var obj in enemyWeapons)
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

    public void InflictEnemyDamage(string damageType)
    {
        isTakingDamage = true;
        if (enemyHealth <0)
        {
            ScoreManager.Instance.ShowYouWonpanel();
        }

        if (damageType == "isLight")
        {
            StartCoroutine(PlayLightReactAnimation());
            StopCoroutine(PlayLightReactAnimation());
            enemyHealth -= 0.1f;
        }
        else if (damageType == "isHeavy")
        {
            StartCoroutine(PlayHeavyReactAnimation());
            StopCoroutine(PlayHeavyReactAnimation());
            enemyHealth -= 0.2f;
        }
        healthBar.fillAmount = enemyHealth;
    }

    IEnumerator PlayLightReactAnimation()
    {
        if(!isPlayingAnotherAnimation) 
        {
            isPlayingAnotherAnimation = true;
            ChangeAnimationState(ENEMY_LIGHTREACT);
            yield return lReactBuffer;
            SetDefaultAnimationState();
            isTakingDamage = false;
            isPlayingAnotherAnimation = false;
        }
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            ChangeAnimationState(ENEMY_HEAVYREACT);
            yield return hReactBuffer;
            this.transform.position = new Vector3(this.transform.position.x + 2.2f, this.transform.position.y, this.transform.position.z);
            SetDefaultAnimationState();
            isTakingDamage = false;
            isPlayingAnotherAnimation = false;
        }
    }

    public void SpecialAttackPlaying()
    {
        isTakingDamage = true;
        StartCoroutine(PlaySpecialAttackReactAnim());
        isTakingDamage = false;
    }

    IEnumerator PlaySpecialAttackReactAnim()
    {
        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            ChangeAnimationState(ENEMY_SPECIALREACT);
            yield return sReactBuffer;
            SetDefaultAnimationState();
            isPlayingAnotherAnimation = false;
        }
    }
}
