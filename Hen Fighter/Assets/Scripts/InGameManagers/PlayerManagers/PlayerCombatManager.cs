using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;

    public bool isAttacking;
    public bool isBlocking;

    void Start()
    {
        clicksCnt = 0;
        playerAnimator = FindObjectOfType<PlayerGamePlayManager>().GetComponent<Animator>();
        weaponCollider = FindObjectsOfType<DamageGeneric>();
        /*weaponCollider[0].GetComponentInChildren<DamageGeneric>().gameObject.SetActive(false);
        weaponCollider[1].GetComponentInChildren<DamageGeneric>().gameObject.SetActive(false);*/
    }

    public void OnLightAttackBtnPressed()
    {
        StartCoroutine(LightAttack());
        StopCoroutine(LightAttack());
    }

    IEnumerator LightAttack()
    {
        isAttacking = true;
        playerAnimator.SetTrigger("isLightAttack");
        weaponCollider[0].GetComponentInChildren<DamageGeneric>().gameObject.SetActive(true);
        clicksCnt++;
        //isComboCheck();
        yield return new WaitForSeconds(1f);
    }

    public void OnHeavyAttackBtnPressed()
    {
        StartCoroutine(HeavyAttack());
        StopCoroutine(HeavyAttack());
    }

    IEnumerator HeavyAttack()
    {
        isAttacking = true;
        playerAnimator.SetTrigger("isHeavyAttack");
        weaponCollider[1].GetComponentInChildren<DamageGeneric>().gameObject.SetActive(true);
        clicksCnt++;
        //isComboCheck();
        yield return new WaitForSeconds(1f);
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
            isAttacking = true;
            playerAnimator.SetTrigger("isComboAttack");
            clicksCnt = 0;
        }
    }
}
