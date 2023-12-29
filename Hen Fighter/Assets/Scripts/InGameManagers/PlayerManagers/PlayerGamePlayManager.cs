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
    AudioManager audioManager;

    Joystick joystick;

    Image healthBar;

    [HideInInspector]
    public Animator playerAnimator;

    int speed;

    [HideInInspector]
    public float playerHealth;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking, isTakingDamage, isSpecialAttack, isPlayingAnotherAnimation, canPerformCombat;

    private AudioSource ClawSound;

    [SerializeField]
    GameObject particleObject;
    private ParticleSystem featherParticle;

    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();
        uiManager = FindObjectOfType<UIManager>();

        particleObject = GameObject.FindWithTag("Player Particles");
        featherParticle = particleObject.GetComponent<ParticleSystem>();

        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        playerHealth = 1f;
    }

    void Update()
    {
        CheckMovement();
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
                UpdateMovementParameters(joystick.Horizontal * 1.5f);
            }   
            else if(joystick.Horizontal > 0.1f)
            {
                playerAnimator.SetBool("inMotion", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.5f);
            } 
        }
        else if (joystick.Horizontal < -0.1f)
        {
            if(joystick.Horizontal < -0.3f)
            {
                playerAnimator.SetBool("inMotion", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 1.5f);
            }
            else if(joystick.Horizontal < -0.1f)
            {
                playerAnimator.SetBool("inMotion", true);
                playerAnimator.SetFloat("joystickDragHorizontal", joystick.Horizontal);
                UpdateMovementParameters(joystick.Horizontal * 0.5f);
            }
        }

        //For Player Jump Operation
        else if (joystick.Vertical > 0.3f)
        {
            playerRb.AddForce(new Vector3(0f, 4.0f, 0f) * 3.0f, ForceMode.Impulse);
            playerAnimator.SetFloat("joystickDragVertical", joystick.Vertical);
            playerAnimator.SetBool("isJumping", true);
        }

        //For Player Crouch Operations
        else if (joystick.Vertical < -0.3f)
        {
            playerAnimator.SetFloat("joystickDragVertical", joystick.Vertical);
            playerAnimator.SetBool("isCrouching", true);
        }

        else
        {
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
                uiManager.PlayerLightFX();
                playerHealth -= 0.02f;
            }
            else if (damageType == "isHeavy")
            {
                StartCoroutine(PlayHeavyReactAnimation());
                featherParticle.Play();
                StopCoroutine(PlayHeavyReactAnimation());
                uiManager.PlayerHeavyFX();
                playerHealth -= 0.04f;
            }
            healthBar.fillAmount = playerHealth;
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
        }
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        playerAnimator.SetTrigger("isHeavyReact");
        yield return new WaitForSeconds(0.5f);
        this.transform.position = new Vector3(this.transform.position.x - 1.2f, this.transform.position.y, this.transform.position.z);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f);
        ScoreManager.Instance.ShowGameOverPanel();
    }
}
