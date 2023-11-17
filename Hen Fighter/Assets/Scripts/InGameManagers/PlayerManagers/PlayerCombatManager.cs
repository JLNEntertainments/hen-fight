using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    static int clicksCnt;

    [SerializeField]
    GameObject[] weaponCollider;

    public bool isAttacking;
    public bool isBlocking;

    void Start()
    {
        clicksCnt = 0;
        playerAnimator = FindObjectOfType<PlayerGamePlayManager>().GetComponent<Animator>();
        weaponCollider = GameObject.FindGameObjectsWithTag("PlayerWeapon");
        weaponCollider[0].SetActive(false);
        weaponCollider[1].SetActive(false);
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
        weaponCollider[1].SetActive(true);
        clicksCnt++;
        //isComboCheck();
        yield return new WaitForSeconds(1f);
        weaponCollider[1].SetActive(false);
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
        weaponCollider[0].SetActive(true);
        clicksCnt++;
        //isComboCheck();
        yield return new WaitForSeconds(1f);
        weaponCollider[0].SetActive(false);
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
