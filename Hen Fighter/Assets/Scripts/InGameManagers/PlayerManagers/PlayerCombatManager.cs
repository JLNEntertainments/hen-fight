using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;

    [HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking;
    public bool isAttacking;
    float currentAttackTime, defaultAttackTime;

    void Start()
    {
        playerAnimator = FindObjectOfType<PlayerGamePlayManager>().GetComponent<Animator>();
        weaponCollider = playerAnimator.GetComponentsInChildren<DamageGeneric>();

        clicksCnt = 0;
        defaultAttackTime = 1f;
        currentAttackTime = defaultAttackTime;

        TurnOffAttackpoints();
    }

    void Update()
    {
        currentAttackTime += Time.deltaTime;
    }

    public void OnLightAttackBtnPressed()
    {
        StartCoroutine(LightAttack());
        StopCoroutine(LightAttack());
    }

    IEnumerator LightAttack()
    {
        if (currentAttackTime > defaultAttackTime)
        {
            isAttacking = true;
            isLightAttack = true;
            isHeavyAttack = false;
            clicksCnt++;
            //isComboCheck();
            PlayAttackAnimation(isHeavyAttack, isLightAttack);
            currentAttackTime = 0;
            yield return new WaitForSeconds(1f); //use waitforframeends

            PlayerGamePlayManager.Instance.ResetAnimationState();
            isAttacking = false;
        }
    }

    public void OnHeavyAttackBtnPressed()
    {
        StartCoroutine(HeavyAttack());
        StopCoroutine(HeavyAttack());
    }

    IEnumerator HeavyAttack()
    {
        if (currentAttackTime > defaultAttackTime)
        {
            isAttacking = true;
            isHeavyAttack = true;
            isLightAttack = false;
            clicksCnt++;
            //isComboCheck();
            PlayAttackAnimation(isHeavyAttack, isLightAttack);
            currentAttackTime = 0;
            yield return new WaitForSeconds(1.2f);
            PlayerGamePlayManager.Instance.ResetAnimationState();
            PlayerGamePlayManager.Instance.transform.position = new Vector3(PlayerGamePlayManager.Instance.transform.position.x + 1.85f, PlayerGamePlayManager.Instance.transform.position.y, PlayerGamePlayManager.Instance.transform.position.z);
            isAttacking = false;
        }
    }

    public void OnBlockAttackBtnPressed()
    {
        isBlocking = true;
        PlayerGamePlayManager.Instance.ChangeAnimationState(PlayerGamePlayManager.PLAYER_BLOCK);
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
                PlayerGamePlayManager.Instance.ChangeAnimationState(PlayerGamePlayManager.PLAYER_LIGHTATTACK);
                return;
            }
            else if(heavyAttack && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                PlayerGamePlayManager.Instance.ChangeAnimationState(PlayerGamePlayManager.PLAYER_HEAVYATTACK);
                return;
            }
        }
    }
}
