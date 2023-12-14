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
    public bool isHeavyAttack, isLightAttack, isBlocking, isTakingDamage, isSpecialAttack, isPlayingAnotherAnimation;

    //Animation States
    [HideInInspector]
    public string PLAYER_IDLE, PLAYER_WALK, PLAYER_RUN, PLAYER_BACKWALK, PLAYER_LIGHTATTACK, PLAYER_LIGHTATTACKTOP, PLAYER_HEAVYATTACK, PLAYER_BLOCK, PLAYER_JUMP, PLAYER_LIGHTREACT, PLAYER_HEAVYREACT, PLAYER_CROUCH, PLAYER_SPECIALATTACK, PLAYER_DEATH;

    

    void Start()
    {
        enemyGamePlayManager = FindObjectOfType<EnemyGamePlayManager>();
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();

        playerAnimator = this.GetComponentInChildren<Animator>();
        playerRb = this.GetComponent<Rigidbody>();

        speed = 2;
        playerHealth = 0.2f;

        PLAYER_IDLE = "Idle";
        PLAYER_WALK = "Walking";
        PLAYER_RUN = "Run";
        PLAYER_BACKWALK = "BackWalk";
        PLAYER_LIGHTATTACK = "LightAttack";
        PLAYER_HEAVYATTACK = "HeavyAttack";
        PLAYER_BLOCK = "Block";
        PLAYER_JUMP = "Jump";
        PLAYER_LIGHTREACT = "LightReact";
        PLAYER_LIGHTATTACKTOP = "LightAttackTop";
        PLAYER_HEAVYREACT = "HeavyReact";
        PLAYER_CROUCH = "Crouch";
        PLAYER_SPECIALATTACK = "SpecialAttack";
        PLAYER_DEATH = "DeathReact";
    }

    void Update()
    {
        CheckMovement();
    }

    void CheckMovement()
    {
        //For Player Movement Operations
        if (joystick.Horizontal > 0.2f)
        {
            if (joystick.Horizontal > 0.5f)
            {
                ChangeAnimationState(PLAYER_RUN);
                UpdateMovementParameters(joystick.Horizontal * 2);
            }   
            else
            {
                ChangeAnimationState(PLAYER_WALK);
                UpdateMovementParameters(joystick.Horizontal * 2);
            } 
        }
        else if (joystick.Horizontal < -0.2f)
        {
            ChangeAnimationState(PLAYER_BACKWALK);
            UpdateMovementParameters(joystick.Horizontal);
        }
        //For Player Jump Operation
        else if (joystick.Vertical > 0.2f)
        {
            playerRb.AddForce(new Vector3(0f, 4.0f, 0f) * 3.0f, ForceMode.Impulse);
            ChangeAnimationState(PLAYER_JUMP);
        }
        //For Player Crouch Operations
        else if (joystick.Vertical < -0.2f)
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

    public void SetDefaultAnimationState()
    {
        playerAnimator.Play(PLAYER_IDLE);
        currentAnimaton = PLAYER_IDLE;
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
            isTakingDamage = true;

            if (damageType == "isLight" && !isPlayingAnotherAnimation)
            {
                isPlayingAnotherAnimation = true;
                StartCoroutine(PlayLightReactAnimation());
                StopCoroutine(PlayLightReactAnimation());
                playerHealth -= 0.1f;
                isPlayingAnotherAnimation = false;
            }
            else if (damageType == "isHeavy" && !isPlayingAnotherAnimation)
            {
                isPlayingAnotherAnimation = true;
                StartCoroutine(PlayHeavyReactAnimation());
                StopCoroutine(PlayHeavyReactAnimation());
                playerHealth -= 0.2f;
                isPlayingAnotherAnimation = false;
            }

            healthBar.fillAmount = playerHealth;
        }
    }

    IEnumerator PlayLightReactAnimation()
    {
        ChangeAnimationState(PLAYER_LIGHTREACT);
        yield return new WaitForSeconds(0.8f);
        SetDefaultAnimationState();
        isTakingDamage = false;
    }

    IEnumerator PlayHeavyReactAnimation()
    {
        ChangeAnimationState(PLAYER_HEAVYREACT);
        yield return new WaitForSeconds(1.2f);
        SetDefaultAnimationState();
        isTakingDamage = false;
    }
}
