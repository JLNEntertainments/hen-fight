using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;
    PlayerGamePlayManager playerGamePlayManager;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;

    /*[HideInInspector]
    public bool isHeavyAttack, isLightAttack, isBlocking;*/
    public bool isAttacking;
    float currentAttackTime, defaultAttackTime;

    void Start()
    {
        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
        playerAnimator = playerGamePlayManager.GetComponent<Animator>();
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
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            isAttacking = true;
            playerGamePlayManager.isLightAttack = true;
            playerGamePlayManager.isHeavyAttack = false;
            clicksCnt++;
            //isComboCheck();
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
            currentAttackTime = 0;
            yield return new WaitForSeconds(0.5f); //use waitforframeends

            playerGamePlayManager.SetDefaultAnimationState();
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
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            isAttacking = true;
            playerGamePlayManager.isHeavyAttack = true;
            playerGamePlayManager.isLightAttack = false;
            clicksCnt++;
            canHitSpecialAttack();
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
            currentAttackTime = 0;
            yield return new WaitForSeconds(0.8f);
            playerGamePlayManager.SetDefaultAnimationState();
            playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.transform.position.x + 1.85f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
            isAttacking = false;
        }
    }

    public void OnSpecialAttackBtnPressed()
    {
        StartCoroutine(SpecialAttack());
        StopCoroutine(SpecialAttack());
    }

    IEnumerator SpecialAttack()
    {
        if (canHitSpecialAttack())
        {
            isAttacking = true;
            playerGamePlayManager.isSpecialAttack = true;
            playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_SPECIALATTACK);
            yield return new WaitForSeconds(2f); 
            playerGamePlayManager.SetDefaultAnimationState();
            isAttacking = false;
        }
    }

    public void OnBlockAttackBtnPressed()
    {
        StartCoroutine(BlockAttack());
        StopCoroutine (BlockAttack());
    }

    IEnumerator BlockAttack()
    {
        playerGamePlayManager.isBlocking = true;
        playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_BLOCK);
        yield return new WaitForSeconds(1f);
        playerGamePlayManager.SetDefaultAnimationState();
        playerGamePlayManager.isBlocking = false;
    }

    bool canHitSpecialAttack()
    {
        if (clicksCnt == 3 && ScoreManager.Instance.characterStaminaValuePlayer >= (ScoreManager.Instance.characterStaminaValuePlayer/2))
        {
            playerGamePlayManager.isSpecialAttack = true;
            return true;
        }
        else
        {
            return false;
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
                playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_LIGHTATTACK);
                return;
            }
            else if(heavyAttack && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_HEAVYATTACK);
                return;
            }
        }
    }
}
