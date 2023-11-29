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

    int speed;

    bool isGrounded;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    //Animation States
    public const string PLAYER_IDLE = "Idle";
    const string PLAYER_WALK = "Walking";
    const string PLAYER_BACKWALK = "BackWalk";
    public const string PLAYER_LIGHTATTACK = "LightAttack";
    public const string PLAYER_HEAVYATTACK = "HeavyAttack";
    public const string PLAYER_BLOCK = "Block";
    const string PLAYER_HURT = "Hurt";

    void Start()
    {
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();
        
        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();
        speed = 2;
    }

    void Update()
    {
        CheckMovement();
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
            playerAnimator.SetBool("isCrouching", true);
            playerAnimator.SetFloat("joystickDrag", joystick.Vertical);
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

    public void InflictPlayerDamage()
    {
        ChangeAnimationState(PLAYER_HURT);
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
}
