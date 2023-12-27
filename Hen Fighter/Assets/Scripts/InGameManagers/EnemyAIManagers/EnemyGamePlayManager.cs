using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;
    UIManager uiManager;
    AudioManager audioManager;

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
    public bool followPlayer, attackPlayer, isHeavyAttack, isSpecialAttack, isLightAttack, isTakingDamage, isAttacking, isBlocking, isPlayerFound, isPlayingAnotherAnimation;

    public DamageGeneric[] enemyWeapons;

    [SerializeField]
    string currentAnimaton;

    private ParticleSystem particleForPlayer;
    //Animation States
    string ENEMY_READYTOFIGHT, ENEMY_IDLE, ENEMY_WALK, ENEMY_BACKWALK, ENEMY_RUN, ENEMY_BACKRUN, ENEMY_JUMP, ENEMY_JUMPANDFLY, ENEMY_CROUCH, ENEMY_LIGHTATTACK, ENEMY_LIGHTATTACKTOP, ENEMY_HEAVYATTACK, ENEMY_HEAVYATTACKKICK, ENEMY_SPECIALATTACK, ENEMY_BLOCK, ENEMY_LIGHTREACT, ENEMY_HEAVYREACT, ENEMY_DEATHREACT, ENEMY_SPECIALREACT, ENEMY_VICTORYJUMP;

    private AudioSource EnemeyAudio;
    private AudioSource ClawSound;

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
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
        GameObject particleObject = GameObject.FindWithTag("Particles");
        particleForPlayer = particleObject.GetComponent<ParticleSystem>();
        ClawSound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

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

        ENEMY_READYTOFIGHT = "ReadyToFight";
        ENEMY_IDLE = "Idle";
        ENEMY_WALK = "Walking";
        ENEMY_BACKWALK = "BackWalk";
        ENEMY_RUN = "Run";
        ENEMY_BACKRUN = "BackRun";
        ENEMY_JUMP = "Jump";
        ENEMY_JUMPANDFLY = "JumpAndFly";
        ENEMY_CROUCH = "Crouch";
        ENEMY_LIGHTATTACK = "LightAttack";
        ENEMY_LIGHTATTACKTOP = "LightAttackTop";
        ENEMY_HEAVYATTACK = "HeavyAttack";
        ENEMY_HEAVYATTACKKICK = "HeavyAttackKick";
        ENEMY_SPECIALATTACK = "SpecialAttack";
        ENEMY_BLOCK = "Block";
        ENEMY_LIGHTREACT = "LightReact";
        ENEMY_HEAVYREACT = "HeavyReact";
        ENEMY_DEATHREACT = "DeathReact";
        ENEMY_SPECIALREACT = "SpecialReact";
        ENEMY_VICTORYJUMP = "VictoryJump";

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
            enemyAnimator.SetBool("inChaseRange", true);
            transform.LookAt(playerGamePlayManager.transform);
            myBody.velocity = Vector3.left * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                
                followPlayer = true;
            }
        }
    }

    public void UnFollowTarget()
    {
        enemyAnimator.SetTrigger("isBackWalk");
        followPlayer = false;
    }

    public void PrepareAttack()
    {
        myBody.velocity = Vector3.zero;
        enemyAnimator.SetBool("inChaseRange", false);
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
            EnemyAttack();
            playerGamePlayManager.canPerformCombat = false;
        }

        if (!enemyAIDecision.IsPlayerInAttackRange())
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    void EnemyAttack()
    {
        int attack = (Random.Range(0, 2));

        foreach (var obj in enemyWeapons)
        {
            if (attack == 1 && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isLightAttack");
                EnemeyAudio.Play();
                isLightAttack = true;
                isHeavyAttack = false;
                
            }

            if (attack == 0 && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                enemyAnimator.SetTrigger("isHeavyAttack");
                EnemeyAudio.Play();
                isHeavyAttack = true;
                isLightAttack = false;
                //this.transform.position = new Vector3(this.transform.position.x - 1.85f, this.transform.position.y, this.transform.position.z);
            }
        }
    }

    public void Defend()
    {
        isBlocking = true;
        enemyAnimator.SetTrigger("isBlocking");
        isBlocking = false;
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

    public void InflictEnemyDamage(string damageType)
    {
        isTakingDamage = true;
        if (enemyHealth <0)
        {
            ScoreManager.Instance.ShowYouWonpanel();
        }

        if (damageType == "isLight")
        {
            PlayLightReactAnimation();
            uiManager.PlayerFX();
            particleForPlayer.Play();
            enemyHealth -= 0.1f;
        }
        else if (damageType == "isHeavy")
        {
            StartCoroutine(PlayHeavyReactAnimation());
            StopCoroutine(PlayHeavyReactAnimation());
            uiManager.PlayerFX();
            particleForPlayer.Play();
            enemyHealth -= 0.2f;
            
        }
        healthBar.fillAmount = enemyHealth;
    }

    void PlayLightReactAnimation()
    {
        enemyAnimator.SetTrigger("isLightReact");
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        enemyAnimator.SetTrigger("isHeavyReact");
        yield return new WaitForSeconds(0.5f);
        this.transform.position = new Vector3(this.transform.position.x + 2.5f, this.transform.position.y, this.transform.position.z);
    }

    public void SpecialAttackPlaying()
    {
        enemyAnimator.SetTrigger("isSpecialReact");
    }
}
