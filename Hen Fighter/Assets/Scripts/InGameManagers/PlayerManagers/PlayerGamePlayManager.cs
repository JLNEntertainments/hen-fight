using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public EnemyGamePlayManager enemyGamePlayManager;
    UIManager uiManager;
    

    Joystick joystick;

    Image healthBar;
    Image healtBarBack;

    [HideInInspector]
    public Animator playerAnimator;

    [SerializeField]
    Animator[] PlayerFXAnim;

    int speed;

    [HideInInspector]
    public float playerHealth;
    private float lerpSpeed = 0.05f;


    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking, isTakingDamage, isSpecialAttack, isPlayingAnotherAnimation, canPerformCombat;

   

    [SerializeField]
    GameObject particleObject;
    private ParticleSystem featherParticle;


    public AudioClip[] Sounds;
    public string soundTag = "Audio";


    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();
        healtBarBack = GameObject.FindGameObjectWithTag("P_HealthBarBack").GetComponentInChildren<Image>();
        uiManager = FindObjectOfType<UIManager>();
        PlayerFXAnim = this.gameObject.GetComponentsInChildren<Animator>();

        particleObject = GameObject.FindWithTag("Player Particles");
        featherParticle = particleObject.GetComponent<ParticleSystem>();

        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        playerHealth = 1f;
     
        TurnOffPlayerFXObjects();
        GameObject[] soundEmitters = GameObject.FindGameObjectsWithTag(soundTag);
    }

    void Update()
    {
        CheckMovement();
      
    }

    void TurnOffPlayerFXObjects()
    {
        PlayerFXAnim[1].gameObject.SetActive(false);
        PlayerFXAnim[2].gameObject.SetActive(false);
    }

    public void PlayerHeavyFX()
    {
        StartCoroutine(PlayPlayerHeavyFX());
        StopCoroutine(PlayPlayerHeavyFX());
    }

    IEnumerator PlayPlayerHeavyFX()
    {
        PlayerFXAnim[2].gameObject.SetActive(true);
        PlayerFXAnim[2].Play("HeavyAttackAnim");
        yield return new WaitForSeconds(0.4f);
        PlayerFXAnim[2].gameObject.SetActive(false);
    }

    public void PlayerLightFX()
    {
        StartCoroutine(PlayPlayerLightFX());
        StopCoroutine(PlayPlayerLightFX());
    }

    IEnumerator PlayPlayerLightFX()
    {
        yield return new WaitForSeconds(0.4f);
        PlayerFXAnim[1].gameObject.SetActive(true);
        PlayerFXAnim[1].Play("EnemyBeekAtack");
        yield return new WaitForSeconds(0.3f);
        PlayerFXAnim[1].gameObject.SetActive(false);
    }

    void CheckMovement()
    {
        //For Player Movement Operations
        if (joystick.Horizontal > 0.1f)
        {
            if (joystick.Horizontal > 0.3f)
            {
                playerAnimator.SetBool("inMotion", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.8f);
            }
            else if (joystick.Horizontal > 0.1f && joystick.Horizontal < 0.3f)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.2f);
            }
        }

        else if (joystick.Horizontal < -0.1f)
        {
            if (joystick.Horizontal < -0.3f)
            {
                playerAnimator.SetBool("inMotion", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.8f);
            }
            else if (joystick.Horizontal < -0.1f)
            {
                playerAnimator.SetBool("isBackWalking", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.2f);
            }
        }

        //For Player Jump Operation
        else if (joystick.Vertical > 0.3f)
        {
            playerAnimator.SetFloat("joystickDragHorizontal", 0);
            playerRb.AddForce(new Vector3(0f, 4.0f, 0f) * 3.0f, ForceMode.Impulse);
            playerAnimator.SetFloat("joystickDragVertical", joystick.Vertical);
            playerAnimator.SetBool("isJumping", true);
        }

        //For Player Crouch Operations
        else if (joystick.Vertical < -0.3f)
        {
            playerAnimator.SetFloat("joystickDragHorizontal", 0);
            playerAnimator.SetFloat("joystickDragVertical", joystick.Vertical);
            playerAnimator.SetBool("isCrouching", true);
        }

        else
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isBackWalking", false);
            playerAnimator.SetBool("inMotion", false);
            playerAnimator.SetBool("isCrouching", false);
            playerAnimator.SetBool("isJumping", false);
            playerAnimator.SetFloat("joystickDragHorizontal", 0);
            playerAnimator.SetFloat("joystickDragVertical", 0);
        }
    }

    void UpdateMovementParameters(float horizontal)
    {
        Vector2 move_position = transform.position;
        playerAnimator.SetFloat("joystickDrag", horizontal);
        move_position.x += horizontal * speed * Time.deltaTime;
        this.transform.position = new Vector3(move_position.x, this.transform.position.y, this.transform.position.z);
    }

    public void InflictPlayerDamage(string damageType)
    {
        if (!isPlayingAnotherAnimation)
        {
            isPlayingAnotherAnimation = true;
            if (damageType == "isLight")
            {
                playerAnimator.SetTrigger("isLightReact");
                featherParticle.Play();
                PlayerLightFX();
                playerHealth -= 0.02f;
            }
            else if (damageType == "isHeavy")
            {
                StartCoroutine(PlayHeavyReactAnimation());
                featherParticle.Play();
                StopCoroutine(PlayHeavyReactAnimation());
                PlayerHeavyFX();
                playerHealth -= 0.04f;
            }
            else if (damageType == "isSpecialAttack")
            {
                featherParticle.Play();
                PlayerLightFX();
                playerHealth -= 0.05f;
            }
            healthBar.fillAmount = playerHealth;

            // Schedule a coroutine to decrease healtBarBack after a delay of 2 seconds
            StartCoroutine(DelayedDecreaseHealtBarBack(1.0f));


            isPlayingAnotherAnimation = false;
        }
        else
            return;

        if(playerHealth <= 0f)
        {
            playerAnimator.SetTrigger("isDeathReact");
            enemyGamePlayManager.enemyAnimator.SetTrigger("hasWon");
            StartCoroutine(ShowGameOverPanel());
            StopCoroutine(ShowGameOverPanel());

            StartCoroutine(TestGamonejctShow());
            StopCoroutine(TestGamonejctShow());

        }
    }


    // Coroutine to decrease healtBarBack after a delay
    private IEnumerator DelayedDecreaseHealtBarBack(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Now call the coroutine to gradually decrease fill amount over time
        float targetFillAmountBack = playerHealth;  // You may adjust this based on your requirement
        StartCoroutine(DecreaseFillAmountOverTime(healtBarBack, targetFillAmountBack));
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


    //For Random Sounds
    public void PlayRandomSound()
    {
        if (Sounds.Length > 0)
        {
            // Pick a random sound from the array
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

    IEnumerator PlayHeavyReactAnimation()
    {
        playerAnimator.SetTrigger("isHeavyReact");
        yield return new WaitForSeconds(0.5f);
        this.transform.position = new Vector3(this.transform.position.x + 2f, this.transform.position.y, this.transform.position.z);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        ScoreManager.Instance.ShowGameOverPanel();
        ScoreManager.Instance.TestGamonejctShow();
    }

    IEnumerator TestGamonejctShow()
    {
        yield return new WaitForSeconds(2.3f);
       
        ScoreManager.Instance.TestGamonejctShow();
    }



}
