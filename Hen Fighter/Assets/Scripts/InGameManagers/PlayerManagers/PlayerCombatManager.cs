using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;

    bool isHeavyAttack, isLightAttack, isBlocking;
    float currentAttackTime, defaultAttackTime;

    void Start()
    {
        clicksCnt = 0;
        playerAnimator = FindObjectOfType<PlayerGamePlayManager>().GetComponent<Animator>();
        weaponCollider = playerAnimator.GetComponentsInChildren<DamageGeneric>();

        defaultAttackTime = 1f;
        currentAttackTime = defaultAttackTime;

        //weaponCollider = FindObjectsOfType<DamageGeneric>();
        TurnOffAttackpoints();
    }

    void Update()
    {
        currentAttackTime += Time.deltaTime;
    }

    public void OnLightAttackBtnPressed()
    {
        if(currentAttackTime > defaultAttackTime)
        {
            isLightAttack = true;
            isHeavyAttack = false;
            clicksCnt++;
            //isComboCheck();
            PlayAttackAnimation(isHeavyAttack, isLightAttack);
            currentAttackTime = 0;
        } 
    }

    public void OnHeavyAttackBtnPressed()
    {
        if(currentAttackTime > defaultAttackTime ) 
        {
            isHeavyAttack = true;
            isLightAttack = false;
            clicksCnt++;
            //isComboCheck();
            PlayAttackAnimation(isHeavyAttack, isLightAttack);
            currentAttackTime = 0;
        }
    }

    public void OnBlockAttackBtnPressed()
    {
        isBlocking = true;
        playerAnimator.SetTrigger("isBlocking");
        clicksCnt = 0;
    }

    void isComboCheck()
    {
        if (clicksCnt == 3)
        {
            playerAnimator.SetTrigger("isComboAttack");
            clicksCnt = 0;
        }
    }

    void TurnOffAttackpoints()
    {
        foreach(var obj in weaponCollider)
            obj.gameObject.SetActive(false);
    }

    void PlayAttackAnimation(bool heavyAttack, bool lightAttack)
    {
        foreach(var obj in weaponCollider)
        {
            if(lightAttack && obj.gameObject.CompareTag("Beak"))
            {
                obj.gameObject.SetActive(true);
                playerAnimator.SetTrigger("isLightAttack");
                return;
            }
            else if(heavyAttack && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                playerAnimator.SetTrigger("isHeavyAttack");
                return;
            }
        }
    }
}
