using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField]
    Joystick joystick;

    Animator playerAnimator;
    
    float rotationSpeed;
    int speed;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        rotationSpeed = 20f;
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
            transform.Translate(0, 0, movementInput);
            playerAnimator.SetBool("isStatic", true);
            transform.Rotate(0, rotationInput, 0);
        }
        else
        {
            playerAnimator.SetBool("isStatic", false);
        }
    }
}
