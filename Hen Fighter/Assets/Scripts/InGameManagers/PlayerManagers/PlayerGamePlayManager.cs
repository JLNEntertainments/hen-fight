using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGamePlayManager : MonoBehaviour
{
    Joystick joystick;

    Image healthBar;
    
    Animator playerAnimator;

    int speed;

    bool isGrounded;

    Rigidbody playerRb;

    void Start()
    {
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        healthBar = GameObject.FindGameObjectWithTag("P_HealthBar").GetComponentInChildren<Image>();
        
        playerAnimator = this.GetComponent<Animator>();
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
            playerAnimator.SetBool("inMotion", false);
            playerAnimator.SetBool("inMotionBackwards", true);
            UpdateMovementParameters(joystick.Horizontal);
        }
        else if(joystick.Horizontal < -0.5f) 
        {
            playerAnimator.SetBool("inMotion", true);
            playerAnimator.SetBool("inMotionBackwards", false);
            UpdateMovementParameters(joystick.Horizontal);
        }
        else
        {
            playerAnimator.SetBool("inMotion", false);
            playerAnimator.SetBool("inMotionBackwards", false);
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
        transform.position = new Vector3(move_position.x, this.transform.position.y, this.transform.position.z);
    }

    public void InflictPlayerDamage()
    {
        playerAnimator.SetInteger("isHurt", 1);
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
}
