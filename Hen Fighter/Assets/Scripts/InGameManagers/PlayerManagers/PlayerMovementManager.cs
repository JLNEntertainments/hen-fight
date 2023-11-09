using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    Joystick joystick;

    Animator playerAnimator;
    
    float rotationSpeed;
    int speed;

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<VariableJoystick>().GetComponent<VariableJoystick>();
        playerAnimator = this.GetComponent<Animator>();
        rotationSpeed = 30f;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    void CheckMovement()
    {
        float rotationInput = joystick.Horizontal * rotationSpeed * Time.deltaTime;
        float movementInput = joystick.Vertical * speed * Time.deltaTime;

        if ((rotationInput > 0 || rotationInput < 0 || movementInput > 0))
        {
            playerAnimator.SetBool("inMotion", true);
            
            if (joystick.Vertical <= 0.5f)
            {
                playerAnimator.SetFloat("joystickDrag", joystick.Vertical);
            }
            else if (joystick.Vertical >= 0.5f)
            {
                playerAnimator.SetFloat("joystickDrag", joystick.Vertical);
            }

            transform.Translate(0, 0, movementInput);
            transform.Rotate(0, rotationInput, 0);
        }
        else
        {
            playerAnimator.SetBool("inMotion", false);
        }
    }
}
