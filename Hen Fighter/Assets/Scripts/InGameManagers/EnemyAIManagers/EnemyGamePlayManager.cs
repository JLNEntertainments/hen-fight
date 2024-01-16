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


   

    [HideInInspector]
    public EnemyAIDecision enemyAIDecision;

    [HideInInspector]
    public Animator enemyAnimator;

    [SerializeField]
    Animator[] EnemyFXAnim;

    Rigidbody myBody;

    Image healthBar;
    Image HealthBarBack;
    private float lerpSpeed = 0.01f;

    private Coroutine decreaseFillCoroutine;

    [HideInInspector]
    public float enemyHealth, attack_Distance, current_Stamina_Regen_Time, default_Stamina_Regen_Time, default_Attack_Time, current_Attack_Time, enemy_Start, enemy_Stamina, block_Attack_Time, enemy_Unfollow_Time;

    [HideInInspector]
    public bool followPlayer, attackPlayer, unfollowTarget, isHeavyAttack, isSpecialAttack, isLightAttack, isTakingDamage, isAttacking, isBlocking, isPlayerFound, isPlayingAnotherAnimation;

    public DamageGeneric[] enemyWeapons;

    [SerializeField]
    string currentAnimaton;

    [SerializeField]
    GameObject particleObject;
    private ParticleSystem featherParticle;


    public AudioClip[] Sounds;
    public string soundTag = "Audio";
    string ENEMY_IDLE;
    

    int randomLightAttack, randomHeavyAttack;


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
        HealthBarBack = GameObject.FindGameObjectWithTag("E_HealthBarBack").GetComponentInChildren<Image>();
        uiManager = FindObjectOfType<UIManager>();
    
        particleObject = GameObject.FindWithTag("Enemy Particles");
        featherParticle = particleObject.GetComponent<ParticleSystem>();
        GameObject[] soundEmitters = GameObject.FindGameObjectsWithTag(soundTag);
        EnemyFXAnim = this.gameObject.GetComponentsInChildren<Animator>();

        //speed = 2f;
        //enemyHealth = 1f;
        attack_Distance = 3f;

        enemy_Stamina = ScoreManager.Instance.characterStaminaValueEnemy;

        default_Attack_Time = 4f;
        default_Stamina_Regen_Time = 8f;
        current_Attack_Time = default_Attack_Time;
        current_Stamina_Regen_Time = 0;
        enemy_Start = 0;

       // EnemeyAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        TurnOffAttackpoints();
        TurnOffEnemyFXObjects();

        ENEMY_IDLE = "Idle";

        //InvokeRepeating("UnfollowTarget", 12f, 5f);
    }

    void Update()
    {
        //enemy_Unfollow_Time += Time.deltaTime;

        randomLightAttack = Random.Range(0, 2);
        randomHeavyAttack = Random.Range(0, 2);

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
            if(!isPlayingAnotherAnimation)
            {
                isPlayingAnotherAnimation = true;
                unfollowTarget = false;
                PlayAnimation("Walking");
            }
            else
                SetDefaultAnimationState();

            /*enemyAnimator.SetBool("inChaseRange", true);
            transform.LookAt(playerGamePlayManager.transform);
            myBody.velocity = Vector3.left * 2f;
            if (myBody.velocity.sqrMagnitude != 0)
            {
                followPlayer = true;
            }*/
        }
    }

    public void UnFollowTarget()
    {
        followPlayer = false;
        attackPlayer = false;
        unfollowTarget = true;

        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            PlayAnimation("BackWalk");
        }
        else
            SetDefaultAnimationState();

        isPlayingAnotherAnimation = false;
    }

    IEnumerator UnFollowAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        enemyAnimator.SetBool("BackWalk", false);
        
    }

    public void PrepareAttack()
    {
        myBody.velocity = Vector3.zero;
        enemyAnimator.SetBool("inChaseRange", false);
        followPlayer = false;
        attackPlayer = true;
        unfollowTarget = false;
    }

    public void Attack()
    {
        if (!attackPlayer)
            return;

        if (!playerGamePlayManager.canPerformCombat && !playerGamePlayManager.isPlayingAnotherAnimation)
        {
            playerGamePlayManager.canPerformCombat = playerGamePlayManager.isPlayingAnotherAnimation = true;
            EnemyAttack();
            playerGamePlayManager.canPerformCombat = playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
        else
            SetDefaultAnimationState();

        if (!enemyAIDecision.IsPlayerInAttackRange())
        {
            attackPlayer = false;
            followPlayer = true;
            unfollowTarget = false;
        }
    }

    void EnemyAttack()
    {
        int attack = (Random.Range(0, 3));
        foreach (var obj in enemyWeapons)
        {
            if (attack == 1 && obj.gameObject.CompareTag("Beak") && canHitLightAttack())
            {
                obj.gameObject.SetActive(true);
                PlayAnimation("LightAttack");

                /*if (randomLightAttack == 0)
                {
                    enemyAnimator.SetTrigger("isLightAttack");
                    enemyAnimator.SetInteger("LightAttackIndex", 1);
                }
                else
                {
                    enemyAnimator.SetTrigger("isLightAttack");
                    enemyAnimator.SetInteger("LightAttackIndex", 2);
                }
                PlayRandomSound();
                isLightAttack = true;
                isHeavyAttack = false;*/
            }
            else if ((attack == 0 || attack == 2) && obj.gameObject.CompareTag("Foot") && canHitHeavyAttack())
            {
                obj.gameObject.SetActive(true);
                PlayAnimation("HeavyAttack");

                /*if (randomHeavyAttack == 0)
                {
                    enemyAnimator.SetTrigger("isHeavyAttack");
                    enemyAnimator.SetInteger("HeavyAttackIndex", 1);
                }
                else
                {
                    enemyAnimator.SetTrigger("isHeavyAttack");
                    enemyAnimator.SetInteger("HeavyAttackIndex", 2);
                }
                PlayRandomSound();
                isHeavyAttack = true;
                isLightAttack = false;*/
            }
            else if(!canHitHeavyAttack() && !canHitLightAttack())
                enemyAnimator.SetTrigger("isLowStamina");
        }
    }

    private void PlayRandomSound()
    {
        if (Sounds.Length > 0)
        {
            AudioClip randomClip = Sounds[Random.Range(0, Sounds.Length)];
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = randomClip;
            audioSource.Play();
            Destroy(audioSource, randomClip.length);
        }
        else
        {
            Debug.LogWarning("No sound clips assigned to the AudioManager.");
        }
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
        PlayAnimation("Defend");
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
                /*PlayLightReactAnimation();
                featherParticle.Play();
                EnemyLightFX();
                ScoreManager.Instance.enemyHealth -= 0.02f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
                isPlayingAnotherAnimation = false;*/

                PlayAnimation("LightReact");
                PlayRandomSound();
                // Add this line to start the coroutine
                StartCoroutine(DelayedDecreaseHealtBarBack(0.01f));
            }
            else if (damageType == "isHeavy")
            {
                /*enemyAnimator.SetTrigger("isHeavyReact");
                featherParticle.Play();
                EnemyHeavyFX();
                StartCoroutine(PlayHeavyReactAnimation());
                StopCoroutine(PlayHeavyReactAnimation());
                ScoreManager.Instance.enemyHealth -= 0.04f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
                isPlayingAnotherAnimation = false;*/

                PlayAnimation("HeavyReact");
                PlayRandomSound();
                // Add this line to start the coroutine
                StartCoroutine(DelayedDecreaseHealtBarBack(0.01f));
            }
            else if(damageType == "isSpecialAttack")
            {
                /*ScoreManager.Instance.enemyHealth -= 0.1f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;*/

                PlayAnimation("SpecialReact");
                PlayRandomSound();
                // Add this line to start the coroutine
                StartCoroutine(DelayedDecreaseHealtBarBack(0.01f));
               
            }

        }
        else
           SetDefaultAnimationState();

        if (ScoreManager.Instance.enemyHealth <= 0f)
        {
            PlayAnimation("DeathReact");

            /*enemyAnimator.SetTrigger("isDeathReact");
            playerGamePlayManager.playerAnimator.SetTrigger("hasWon");
            StartCoroutine(ShowGameOverPanel());
            StopCoroutine(ShowGameOverPanel());

            StartCoroutine(TestGamonejctShow());
            StopCoroutine(ShowGameOverPanel());*/
        }
        
    }

    // Coroutine to decrease healtBarBack after a delay
    private IEnumerator DelayedDecreaseHealtBarBack(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Now call the coroutine to gradually decrease fill amount over time
        float targetFillAmountBack = enemyHealth;  // You may adjust this based on your requirement
        StartCoroutine(DecreaseFillAmountOverTime(HealthBarBack, targetFillAmountBack));
    }

    // Coroutine to gradually decrease fill amount over time
    private IEnumerator DecreaseFillAmountOverTime(Image image, float targetFillAmount)
    {
        float duration = Mathf.Abs(image.fillAmount - targetFillAmount) / lerpSpeed;
        float elapsedTime = 0.2f;
        float startFillAmount = image.fillAmount;

        while (elapsedTime < duration)
        {
            image.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final fill amount is exactly the target fill amount
        image.fillAmount = targetFillAmount;
    }

    void PlayLightReactAnimation()
    {
        enemyAnimator.SetTrigger("isLightReact");
    }

    IEnumerator PlayHeavyReactAnimation()
    {
       // enemyAnimator.SetTrigger("isHeavyReact");
        yield return new WaitForSeconds(0.5f);
        // this.transform.position = new Vector3(this.transform.position.x + 1.2f, this.transform.position.y, this.transform.position.z);
        enemyAnimator.SetTrigger("isIdle");
    }

    public void SpecialAttackPlaying()
    {
        enemyAnimator.SetTrigger("isSpecialReact");
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2f);
        ScoreManager.Instance.ShowYouWonpanel();
    }

    IEnumerator TestGamonejctShow()
    {
        yield return new WaitForSeconds(2.3f);
        ScoreManager.Instance.TestGamonejctShow();
    }

    public void PlayAnimation(string animationName)
    {
        switch(animationName) 
        {

            case "LightAttack":

                if (randomLightAttack == 0)
                {
                    enemyAnimator.SetTrigger("isLightAttack");
                    enemyAnimator.SetInteger("LightAttackIndex", 1);
                }
                else
                {
                    enemyAnimator.SetTrigger("isLightAttack");
                    enemyAnimator.SetInteger("LightAttackIndex", 2);
                }
                
                isLightAttack = true;
                isHeavyAttack = false;

                break;


            case "HeavyAttack":

                if (randomHeavyAttack == 0)
                {
                    enemyAnimator.SetTrigger("isHeavyAttack");
                    enemyAnimator.SetInteger("HeavyAttackIndex", 1);
                }
                else
                {
                    enemyAnimator.SetTrigger("isHeavyAttack");
                    enemyAnimator.SetInteger("HeavyAttackIndex", 2);
                }
                
                isHeavyAttack = true;
                isLightAttack = false;

                break;


            case "HeavyReact":

                enemyAnimator.SetTrigger("isHeavyReact");
                featherParticle.Play();
                EnemyHeavyFX();
                StartCoroutine(PlayHeavyReactAnimation());
                StopCoroutine(PlayHeavyReactAnimation());
                ScoreManager.Instance.enemyHealth -= 0.04f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
                isPlayingAnotherAnimation = false;

                break;


            case "LightReact":

                PlayLightReactAnimation();
                featherParticle.Play();
                EnemyLightFX();
                ScoreManager.Instance.enemyHealth -= 0.02f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;
                isPlayingAnotherAnimation = false;

                break;


            case "SpecialReact":

                enemyAnimator.SetTrigger("isSpecialReact");
                ScoreManager.Instance.enemyHealth -= 0.1f;
                healthBar.fillAmount = ScoreManager.Instance.enemyHealth;

                break;


            case "DeathReact":

                enemyAnimator.SetTrigger("isDeathReact");
                playerGamePlayManager.playerAnimator.SetTrigger("hasWon");
                StartCoroutine(ShowGameOverPanel());
                StopCoroutine(ShowGameOverPanel());

                StartCoroutine(TestGamonejctShow());
                StopCoroutine(ShowGameOverPanel());

                break;


            case "BackWalk":

                float temp_enemy_Unfollow_Time = 0;
                enemy_Unfollow_Time = 0;
                while (temp_enemy_Unfollow_Time < 0.1f && enemyAIDecision.backWalkToggle)
                {
                    enemyAnimator.SetBool("inChaseRange", false);
                    enemyAnimator.SetBool("BackWalk", true);
                    myBody.velocity = Vector3.right * 0.5f;
                    temp_enemy_Unfollow_Time += 1;

                    if (enemy_Unfollow_Time == 2)
                    {
                        enemyAIDecision.backWalkToggle = false;
                        break;
                    }
                        

                    Debug.Log("-----" + temp_enemy_Unfollow_Time);
                }
                
                break;


            case "Walking":

                enemyAnimator.SetBool("inChaseRange", true);
                enemyAnimator.SetBool("BackWalk", false);
                transform.LookAt(playerGamePlayManager.transform);
                myBody.velocity = Vector3.left * 2f;
                if (myBody.velocity.sqrMagnitude != 0)
                {
                    followPlayer = true;
                    unfollowTarget = false;
                }
                isPlayingAnotherAnimation = false;

                break;

            case "Defend":

                isBlocking = true;
                enemyAnimator.SetTrigger("isBlocking");
                isBlocking = false;

                break;
        }
    }

    public void SetDefaultAnimationState()
    {
        enemyAnimator.Play(ENEMY_IDLE);
        currentAnimaton = ENEMY_IDLE;
    }
}
