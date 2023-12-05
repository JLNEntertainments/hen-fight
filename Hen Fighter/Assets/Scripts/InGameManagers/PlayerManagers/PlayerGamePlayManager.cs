using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGamePlayManager : MonoBehaviour
{
    [HideInInspector]
    public EnemyGamePlayManager enemyGamePlayManager;

    Joystick joystick;

    Image healthBar;
    
    Animator playerAnimator;

    StaminaHandlerManager playerStaminaHandler;

    int speed;

    bool isGrounded;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking;

    float current_Stamina_Regen_Time, default_Stamina_Regen_Time;

    //Animation States
    [HideInInspector]
    public string PLAYER_IDLE, PLAYER_WALK, PLAYER_BACKWALK, PLAYER_LIGHTATTACK, PLAYER_HEAVYATTACK, PLAYER_BLOCK, PLAYER_HURT, PLAYER_CROUCH;

    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();

        playerStaminaHandler = this.GetComponent<StaminaHandlerManager>();
        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        default_Stamina_Regen_Time = 8f;
        current_Stamina_Regen_Time = 0;

        PLAYER_IDLE = "Idle";
        PLAYER_WALK = "Walking";
        PLAYER_BACKWALK = "BackWalk";
        PLAYER_LIGHTATTACK = "LightAttack";
        PLAYER_HEAVYATTACK = "HeavyAttack";
        PLAYER_BLOCK = "Block";
        PLAYER_HURT = "Hurt";
        PLAYER_CROUCH = "Crouch";
    }

    void Update()
    {
        CheckMovement();
        /*if (!PlayerCombatManager.Instance.isAttacking && !isMoving)
            StaminaRegeneration();*/
    }

    void CheckMovement()
    {
        //For Player Movement Operations
        if (joystick.Horizontal > 0.5f) 
        {
            ChangeAnimationState(PLAYER_WALK);
            UpdateMovementParameters(joystick.Horizontal);
        }
        else if(joystick.Horizontal < -0.5f) 
        {
            ChangeAnimationState(PLAYER_BACKWALK);
            UpdateMovementParameters(joystick.Horizontal);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
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

    /*void StaminaRegeneration()
    {
        current_Stamina_Regen_Time += Time.deltaTime;
        if (current_Stamina_Regen_Time >= default_Stamina_Regen_Time && playerStaminaHandler.characterStamina < playerStaminaHandler.maxStamina)
        {
            playerStaminaHandler.IncreaseStamina();
            current_Stamina_Regen_Time = 0;
        }
    }*/

    public void InflictPlayerDamage()
    {
        ChangeAnimationState(PLAYER_HURT);
        //StartCoroutine(PlayHurtAnimation());
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
