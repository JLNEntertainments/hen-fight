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

    [HideInInspector]
    public Animator enemyAnimator;

    [SerializeField]
    Animator[] EnemyFXAnim;

    Rigidbody myBody;

    Image healthBar;

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

    [SerializeField]
    GameObject particleObject;
    private ParticleSystem featherParticle;

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
        particleObject = GameObject.FindWithTag("Enemy Particles");
        featherParticle = particleObject.GetComponent<ParticleSystem>();
        ClawSound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        EnemyFXAnim = this.gameObject.GetComponentsInChildren<Animator>();

        //speed = 2f;
        //enemyHealth = 1f;
        attack_Distance = 2.5f;

        enemy_Stamina = ScoreManager.Instance.characterStaminaValueEnemy;

        default_Attack_Time = 3f;
        default_Stamina_Regen_Time = 8f;
        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;
        enemy_Start = 0;

        EnemeyAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        TurnOffAttackpoints();
        TurnOffEnemyFXObjects();
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

    void TurnOffEnemyFXObjects()
    {
        EnemyFXAnim[1].gameObject.SetActive(false);
        EnemyFXAnim[2].gameObject.SetActive(false);
    }

    public void EnemyLightFX()
    {
        StartCoroutine(PlayEnemyLightFX());
        StopCoroutine(PlayEnemyLightFX());
    }

    IEnumerator PlayEnemyLightFX()
    {
        EnemyFXAnim[1].gameObject.SetActive(true);
        EnemyFXAnim[1].Play("EnemyBeakAttack");
        yield return new WaitForSeconds(0.3f);
        EnemyFXAnim[1].gameObject.SetActive(false);
    }

    public void EnemyHeavyFX()
    {
        StartCoroutine(PlayEnemyHeavyFX());
        StopCoroutine(PlayEnemyHeavyFX());
    }

    IEnumerator PlayEnemyHeavyFX()
    {
        EnemyFXAnim[2].gameObject.SetActive(true);
        EnemyFXAnim[2].Play("HeavyAttackAnim");
        yield return new WaitForSeconds(0.4f);
        EnemyFXAnim[2].gameObject.SetActive(false);
    }

    public void FollowTarget()
    {
        if (enemyAIDecision.IsPlayerInChaseRange())
        {
            enemyAnimator.SetBool("inChaseRange", true);
            transform.LookAt(playerGamePlayManager.transform);
            myBody.velocity = Vector3.left * 2f;
            if (myBody.velocity.sqrMagnitude != 0)
            {
                followPlayer = true;
            }
        }
    }

    public void UnFollowTarget()
    {
        enemyAnimator.SetTrigger("isBackWalk");
        myBody.velocity = Vector3.right * 2f;
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
        
        if(!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            foreach (var obj in enemyWeapons)
            {
                if (attack == 1 && obj.gameObject.CompareTag("Beak") && canHitLightAttack())
                {
                    obj.gameObject.SetActive(true);
                    enemyAnimator.SetTrigger("isLightAttack");
                    EnemeyAudio.Play();
                    isLightAttack = true;
                    isHeavyAttack = false;

                }

                if (attack == 0 && obj.gameObject.CompareTag("Foot") && canHitHeavyAttack())
                {
                    obj.gameObject.SetActive(true);
                    enemyAnimator.SetTrigger("isHeavyAttack");
                    EnemeyAudio.Play();
                    isHeavyAttack = true;
                    isLightAttack = false;
                    this.transform.position = new Vector3(this.transform.position.x - 1.2f, this.transform.position.y, this.transform.position.z);
                }
            }
            isPlayingAnotherAnimation = false;
        }
        else
            return;
    }

    bool canHitLightAttack()
    {
        if (ScoreManager.Instance.characterStaminaValueEnemy >= ScoreManager.Instance.LightAttackDamage)
            return true;
        else
            return false;
    }

    bool canHitHeavyAttack()
    {
        if (ScoreManager.Instance.characterStaminaValueEnemy >= ScoreManager.Instance.HeavyAttackDamage)
            return true;
        else
            return false;
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

    public void TurnOffAttackpoints()
    {
        foreach (var obj in enemyWeapons)
            obj.gameObject.SetActive(false);
    }

    public void InflictEnemyDamage(string damageType)
    {
        isTakingDamage = true;

        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            if (damageType == "isLight")
            {
                PlayLightReactAnimation();
                featherParticle.Play();
                EnemyLightFX();
                ScoreManager.Instance.enemyHealth -= 0.02f;
            }
            else if (damageType == "isHeavy")
            {
                StartCoroutine(PlayHeavyReactAnimation());
                featherParticle.Play();
                StopCoroutine(PlayHeavyReactAnimation());
                EnemyHeavyFX();
                ScoreManager.Instance.enemyHealth -= 0.04f;
            }
            healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
            isPlayingAnotherAnimation = false;
        }
        else
            return;

        if (ScoreManager.Instance.enemyHealth <= 0f)
        {
            enemyAnimator.SetTrigger("isDeathReact");
            playerGamePlayManager.playerAnimator.SetTrigger("hasWon");
            StartCoroutine(ShowGameOverPanel());
            StopCoroutine(ShowGameOverPanel());
        }
    }

    void PlayLightReactAnimation()
    {
        enemyAnimator.SetTrigger("isLightReact");
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        enemyAnimator.SetTrigger("isHeavyReact");
        yield return new WaitForSeconds(0.5f);
        this.transform.position = new Vector3(this.transform.position.x + 1.2f, this.transform.position.y, this.transform.position.z);
    }

    public void SpecialAttackPlaying()
    {
        enemyAnimator.SetTrigger("isSpecialReact");
        ScoreManager.Instance.enemyHealth -= 0.1f;
        healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2f);
        ScoreManager.Instance.ShowYouWonpanel();
    }
}
