using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHandlerManager : MonoBehaviour
{
    GameObject characterObj;
 
    Animator characterAnim;
    public Image StaminaBarImage;

    [Space]
    bool isBlocking;

    [Space]
    [Range(0, 100f)]
    public float characterStamina;

    [HideInInspector]
    public float maxStamina;
    float characterStaminaValue;

    [Space]
    public float LightAttackDamage;
    public float MediumAttackDamage;
    public float HeavyAttackDamage;

    [Space]
    public float BlockDamageOffset;

    void Start()
    {
        characterStamina = 50f;
        maxStamina = 100f;
        characterObj = this.gameObject;
        characterAnim = characterObj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        characterStaminaValue = characterStamina * .01f;
        StaminaBarImage.fillAmount = characterStaminaValue;
    }

    public void LightAtatck()
    {
        if (isBlocking)
        {
            characterStamina -= (LightAttackDamage / BlockDamageOffset);
        }
        else
        characterStamina -= LightAttackDamage;
        characterAnim.SetTrigger("isLightAttack");
    }
    public void MediumAttack()
    {
        if (isBlocking)
        {
            characterStamina -= (MediumAttackDamage / BlockDamageOffset);
        }else
        characterStamina -= MediumAttackDamage;
        characterAnim.SetTrigger("isLightAttack");
    }

    public void HeavyAttack()
    {
        if (isBlocking)
        {
            characterStamina -= (HeavyAttackDamage / BlockDamageOffset);
        }
        else
            characterStamina -= HeavyAttackDamage;
        characterAnim.SetTrigger("isHeavyAttack");
    }

    public void SetBlocking()
    {
        if (isBlocking)
        {
            isBlocking = false;
        }
        else if (!isBlocking)
        {
            isBlocking = true;
        }
    }

    public void IncreaseStamina()
    {
        characterStamina += 10f;
    }
}


