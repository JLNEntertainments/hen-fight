using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGamePlayManager : SingletonGeneric<PlayerGamePlayManager>
{
    Joystick joystick;

    Image healthBar;
    
    Animator playerAnimator;

    StaminaHandlerManager playerStaminaHandler;

    int speed;

    bool isGrounded, isMoving;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    //Animation States
    public const string PLAYER_IDLE = "Idle";
    const string PLAYER_WALK = "Walking";
    const string PLAYER_BACKWALK = "BackWalk";
    public const string PLAYER_LIGHTATTACK = "LightAttack";
    public const string PLAYER_HEAVYATTACK = "HeavyAttack";
    public const string PLAYER_BLOCK = "Block";
    const string PLAYER_HURT = "Hurt";
    const string PLAYER_CROUCH = "Crouch";

    void Start()
    {
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();

        playerStaminaHandler = this.GetComponent<StaminaHandlerManager>();
        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        default_Stamina_Regen_Time = 8f;
        current_Stamina_Regen_Time = 0;
    }

    void Update()
    {
        CheckMovement();
        if (!PlayerCombatManager.Instance.isAttacking && !isMoving)
            StaminaRegeneration();
    }

    void CheckMovement()
    {
        //For Player Movement Operations
        if (joystick.Horizontal > 0.5f) 
        {
            ChangeAnimationState(PLAYER_WALK);
            UpdateMovementParameters(joystick.Horizontal);
            isMoving = true;
        }
        else if(joystick.Horizontal < -0.5f) 
        {
            ChangeAnimationState(PLAYER_BACKWALK);
            UpdateMovementParameters(joystick.Horizontal);
            isMoving = true;
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
            isMoving= false;
        }

        //For Player Jump Operation
        if (joystick.Vertical > 0.5f && isGrounded)
        {
            playerAnimator.SetTrigger("isJumping");
            playerRb.AddForce(new Vector3(0f, 3.0f, 0f) * 3.0f, ForceMode.Impulse);
            isGrounded = false;
        }

        //For Player Crouch Operations
        else if (joystick.Vertical < -0.5f && isGrounded)
        {
            //playerAnimator.SetBool("isCrouching", true);
            //playerAnimator.SetFloat("joystickDrag", joystick.Vertical);
            ChangeAnimationState(PLAYER_CROUCH);
        }
        else
            playerAnimator.SetBool("isCrouching", false);
    }

    void UpdateMovementParameters(float horizontal)
    {
        Vector2 move_position = transform.position;
        playerAnimator.SetFloat("joystickDrag", horizontal);
        move_position.x += horizontal * speed * Time.deltaTime;
        this.transform.position = new Vector3(move_position.x, this.transform.position.y, this.transform.position.z);
    }

    void StaminaRegeneration()
    {
        current_Stamina_Regen_Time += Time.deltaTime;
        if (current_Stamina_Regen_Time >= default_Stamina_Regen_Time && playerStaminaHandler.characterStamina < playerStaminaHandler.maxStamina)
        {
            playerStaminaHandler.IncreaseStamina();
            current_Stamina_Regen_Time = 0;
        }
    }

    public void InflictPlayerDamage()
    {
        StartCoroutine(PlayHurtAnimation());
        //ChangeAnimationState(PLAYER_HURT);
        healthBar.fillAmount -= 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        playerAnimator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    public void ResetAnimationState()
    {
        playerAnimator.Play(PLAYER_IDLE);
        currentAnimaton = PLAYER_IDLE;
    }

    IEnumerator PlayHurtAnimation()
    {
        ChangeAnimationState(PLAYER_HURT);
        yield return new WaitForSeconds(2f);
        ResetAnimationState();
    }
}
