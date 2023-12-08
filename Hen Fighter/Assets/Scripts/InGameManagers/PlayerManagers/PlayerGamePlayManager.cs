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

    int speed;

    [HideInInspector]
    public float playerHealth;

    Rigidbody playerRb;

    [HideInInspector]
    public string currentAnimaton;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking, isTakingDamage;

    //Animation States
    [HideInInspector]
    public string PLAYER_IDLE, PLAYER_WALK, PLAYER_BACKWALK, PLAYER_LIGHTATTACK, PLAYER_HEAVYATTACK, PLAYER_BLOCK, PLAYER_LIGHTREACT, PLAYER_HEAVYREACT, PLAYER_CROUCH;

    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();

        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        playerHealth = 1f;
        PLAYER_IDLE = "Idle";
        PLAYER_WALK = "Walking";
        PLAYER_BACKWALK = "BackWalk";
        PLAYER_LIGHTATTACK = "LightAttack";
        PLAYER_HEAVYATTACK = "HeavyAttack";
        PLAYER_BLOCK = "Block";
        PLAYER_LIGHTREACT = "LightReact";
        PLAYER_HEAVYREACT = "HeavyReact";
        PLAYER_CROUCH = "Crouch";
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
        else if (joystick.Horizontal < -0.5f)
        {
            ChangeAnimationState(PLAYER_BACKWALK);
            UpdateMovementParameters(joystick.Horizontal);
        }
        //For Player Jump Operation
        else if (joystick.Vertical > 0.5f)
        {
            playerAnimator.SetTrigger("isJumping");
            playerRb.AddForce(new Vector3(0f, 3.0f, 0f) * 3.0f, ForceMode.Impulse);
        }
        //For Player Crouch Operations
        else if (joystick.Vertical < -0.5f)
        {
            ChangeAnimationState(PLAYER_CROUCH);
        }
        else
            ChangeAnimationState(PLAYER_IDLE);
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

    public void ResetAnimationState()
    {
        playerAnimator.Play(PLAYER_IDLE);
        currentAnimaton = PLAYER_IDLE;
    }

    public void InflictLightDamage()
    {
        isTakingDamage = true;
        StartCoroutine(PlayLightReactAnimation());
        playerHealth -= 0.1f;
        healthBar.fillAmount = playerHealth;
    }

    IEnumerator PlayLightReactAnimation()
    {
        ChangeAnimationState(PLAYER_LIGHTREACT);
        yield return new WaitForSeconds(0.8f);
        ResetAnimationState();
        isTakingDamage = false;
    }

    public void InflictHeavyDamage()
    {
        isTakingDamage = true;
        StartCoroutine(PlayHeavyReactAnimation());
        playerHealth -= 0.2f;
        healthBar.fillAmount = playerHealth;
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        ChangeAnimationState(PLAYER_HEAVYREACT);
        yield return new WaitForSeconds(1.2f);
        ResetAnimationState();
        isTakingDamage = false;
    }
}
