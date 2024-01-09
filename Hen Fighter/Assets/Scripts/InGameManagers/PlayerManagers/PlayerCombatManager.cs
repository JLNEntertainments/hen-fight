using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;
    UIManager uiManager;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;
    public bool isAttacking;

    float currentAttackTime, defaultAttackTime, remainingStamina;

    int randomLightAttack, randomHeavyAttack;

    public void AssignplayerAttributes()
    {
        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
        playerAnimator = playerGamePlayManager.GetComponent<Animator>();
        weaponCollider = playerAnimator.GetComponentsInChildren<DamageGeneric>();
        uiManager = GetComponent<UIManager>();

        clicksCnt = 0;
        defaultAttackTime = 1f;
        currentAttackTime = defaultAttackTime;

        TurnOffAttackpoints();
    }

    void Update()
    { 
        randomLightAttack = Random.Range(0, 2);
        randomHeavyAttack = Random.Range(0, 2);
        currentAttackTime += Time.deltaTime;

        if (clicksCnt >= 3)
            canHitSpecialAttack();
    }

    public void OnLightAttackBtnPressed()
    {
        if(canHitLightAttack())
        {
            if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
            {
                playerGamePlayManager.isLightAttack = true;
                playerGamePlayManager.isHeavyAttack = false;
                clicksCnt++;
                PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
                playerGamePlayManager.PlayRandomSound();
                currentAttackTime = 0;
                playerGamePlayManager.isPlayingAnotherAnimation = false;
            }
        }
        else
        {
            playerAnimator.SetTrigger("isLowStamina");
        }
    }

    public void OnHeavyAttackBtnPressed()
    {
        if(canHitHeavyAttack())
        {
            if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
            {
                playerGamePlayManager.isHeavyAttack = true;
                playerGamePlayManager.isLightAttack = false;
                clicksCnt++;
                PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
                playerGamePlayManager.PlayRandomSound();
                currentAttackTime = 0;
                playerGamePlayManager.isPlayingAnotherAnimation = false;
            } 
        }
        else
        {
            playerAnimator.SetTrigger("isLowStamina");
        }
    }

    public void OnSpecialAttackBtnPressed()
    {
        playerGamePlayManager.isHeavyAttack = false;
        playerGamePlayManager.isLightAttack = false;

        if (canHitSpecialAttack())
        {
            playerGamePlayManager.isSpecialAttack = true;
            playerAnimator.SetTrigger("isSpecialAttack");
            weaponCollider[1].gameObject.SetActive(true);
            if (playerGamePlayManager.enemyGamePlayManager.enemyAIDecision.IsPlayerInAttackRange())
                playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x - 0.8f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
            else
                playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x - 1.5f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);

            uiManager.specialAttackBtnAnim.SetActive(false);
            clicksCnt = 0;
            playerGamePlayManager.isPlayingAnotherAnimation = false;
            StartCoroutine(SpecialAttackBuffer());
            StopCoroutine(SpecialAttackBuffer());
        }
    }

    IEnumerator SpecialAttackBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        TurnOffAttackpoints();
    }

    public void OnBlockAttackBtnPressed()
    {
        playerGamePlayManager.isBlocking = true;
        playerAnimator.SetTrigger("isBlocking");
        playerGamePlayManager.isBlocking = false;
    }

    bool canHitSpecialAttack()
    {
        remainingStamina = (ScoreManager.Instance.maxStamina / 2);
        if (ScoreManager.Instance.characterStaminaValuePlayer >= remainingStamina && clicksCnt >= 3 && !playerGamePlayManager.isSpecialAttack)
        {
            uiManager.specialAttackBtnAnim.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool canHitLightAttack()
    {
        if (ScoreManager.Instance.characterStaminaValuePlayer >= ScoreManager.Instance.LightAttackDamage)
            return true;
        else
            return false;
    }

    bool canHitHeavyAttack()
    {
        if (ScoreManager.Instance.characterStaminaValuePlayer >= ScoreManager.Instance.HeavyAttackDamage)
            return true;
        else
            return false;
    }

    void TurnOffAttackpoints()
    {
        foreach (var obj in weaponCollider)
            obj.gameObject.SetActive(false);
    }

    void PlayAttackAnimation(bool heavyAttack, bool lightAttack)
    {
        if (!playerGamePlayManager.isPlayingAnotherAnimation)
        {
            playerGamePlayManager.isPlayingAnotherAnimation = true;
            foreach (var obj in weaponCollider)
            {
                if (lightAttack && obj.gameObject.CompareTag("Beak"))
                {
                    if (randomLightAttack == 0)
                    {
                        playerAnimator.SetTrigger("isLightAttack");
                        playerAnimator.SetInteger("LightAttackIndex", 1);
                        obj.gameObject.SetActive(true);
                        return;
                    }
                    else
                    {
                        playerAnimator.SetTrigger("isLightAttack");
                        playerAnimator.SetInteger("LightAttackIndex", 2);
                        obj.gameObject.SetActive(true);
                        return;
                    }
                }
                else if (heavyAttack && obj.gameObject.CompareTag("Foot"))
                {

                    if (randomHeavyAttack == 0)
                    {
                        playerAnimator.SetTrigger("isHeavyAttack");
                        playerAnimator.SetInteger("HeavyAttackIndex", 1);
                        obj.gameObject.SetActive(true);
                        StartCoroutine(HeavyAttackOffset());
                        StopCoroutine(HeavyAttackOffset());
                        return;
                    }
                    else
                    {
                        playerAnimator.SetTrigger("isHeavyAttack");
                        playerAnimator.SetInteger("HeavyAttackIndex", 2);
                        obj.gameObject.SetActive(true);
                        return;
                    }
                }
            }
        }
        else
            return;
    }

    IEnumerator HeavyAttackOffset()
    {
        /*yield return new WaitForSeconds(0.4f);
        playerGamePlayManager.transform.localPosition = new Vector3(playerGamePlayManager.transform.localPosition.x + 1.2f, playerGamePlayManager.transform.localPosition.y, playerGamePlayManager.transform.localPosition.z);*/
        yield return new WaitForSeconds(0.1f);
        TurnOffAttackpoints();
    }
}