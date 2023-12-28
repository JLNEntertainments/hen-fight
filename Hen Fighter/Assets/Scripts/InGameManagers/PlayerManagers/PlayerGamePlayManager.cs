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

    Animator playerAnimator;

    int speed;

    [HideInInspector]
    public float playerHealth;

    float lightReactBuffer, heavyReactBuffer;
    WaitForSeconds lightBuffer, heavyBuffer;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking, isTakingDamage, isSpecialAttack, isPlayingAnotherAnimation, canPerformCombat;

    //Animation States
    [HideInInspector]
    public string PLAYER_READYTOFIGHT, PLAYER_IDLE, PLAYER_WALK, PLAYER_RUN, PLAYER_BACKRUN, PLAYER_BACKWALK, PLAYER_LIGHTATTACK, PLAYER_LIGHTATTACKTOP, PLAYER_HEAVYATTACK, PLAYER_HEAVYATTACKKICK, PLAYER_BLOCK, PLAYER_JUMP, PLAYER_JUMPANDFLY, PLAYER_LIGHTREACT, PLAYER_HEAVYREACT, PLAYER_CROUCH, PLAYER_SPECIALATTACK, PLAYER_SPECIALREACT, PLAYER_DEATH, PLAYER_VICTORYJUMP;

    private AudioSource ClawSound;

    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();
        uiManager = FindObjectOfType<UIManager>();

        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        playerHealth = 1f;
        lightReactBuffer = 0f;
        heavyReactBuffer = 0f;

        lightBuffer = new WaitForSeconds(lightReactBuffer);
        heavyBuffer = new WaitForSeconds(heavyReactBuffer);

        PLAYER_READYTOFIGHT = "ReadyToFight";
        PLAYER_IDLE = "Idle";
        PLAYER_WALK = "Walking";
        PLAYER_BACKWALK = "BackWalk";
        PLAYER_RUN = "Run";
        PLAYER_BACKRUN = "BackRun";
        PLAYER_JUMP = "Jump";
        PLAYER_CROUCH = "Crouch";
        PLAYER_LIGHTATTACK = "LightAttack";
        PLAYER_LIGHTATTACKTOP = "LightAttackTop";
        PLAYER_HEAVYATTACK = "HeavyAttack";
        PLAYER_HEAVYATTACKKICK = "HeavyAttackKick";
        PLAYER_SPECIALATTACK = "SpecialAttack";
        PLAYER_BLOCK = "Block";
        PLAYER_LIGHTREACT = "LightReact";
        PLAYER_HEAVYREACT = "HeavyReact";
        PLAYER_DEATH = "DeathReact";
        PLAYER_SPECIALREACT = "SpecialReact";
        PLAYER_VICTORYJUMP = "VictoryJump";
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
            playerAnimator.SetFloat("joystickDragvertical", 0);
        }
    }

    void UpdateMovementParameters(float horizontal)
    {
        Vector2 move_position = transform.position;
        playerAnimator.SetFloat("joystickDrag", horizontal);
        move_position.x += horizontal * speed * Time.deltaTime;
        this.transform.position = new Vector3(move_position.x, this.transform.position.y, this.transform.position.z);
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        playerAnimator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    public void SetDefaultAnimationState()
    {
        ChangeAnimationState(PLAYER_IDLE);
    }

    public void InflictPlayerDamage(string damageType)
    {
        Debug.Log("----- Health : " + playerHealth);
        if (playerHealth <= 0)
        {
            ChangeAnimationState(PLAYER_DEATH);
            ScoreManager.Instance.ShowGameOverPanel();
            enemyGamePlayManager.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        else
        {
            if (damageType == "isLight")
            {
                playerAnimator.SetTrigger("isLightReact");
                uiManager.PlayFX();
                playerHealth -= 0.1f;
            }
            else if (damageType == "isHeavy")
            {
                playerAnimator.SetTrigger("isHeavyReact");
                uiManager.PlayFX();
                playerHealth -= 0.2f;
            }
            healthBar.fillAmount = playerHealth;
        }
    }
}
