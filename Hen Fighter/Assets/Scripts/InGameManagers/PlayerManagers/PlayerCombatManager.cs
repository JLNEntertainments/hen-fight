using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;

    [HideInInspector]
    public PlayerGamePlayManager playerGamePlayManager;
    UIManager uiManager;

    public  int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;
    public bool isAttacking;

    public TMP_Text HitCountTex;

    float currentAttackTime, defaultAttackTime, remainingStamina;

    int randomLightAttack, randomHeavyAttack;

    public GameObject SuperPowetText;


    private void Start()
    {
        HitCountTex.gameObject.SetActive(false);
    }
    public void AssignplayerAttributes()
    {
        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
        playerAnimator = playerGamePlayManager.GetComponent<Animator>();
        weaponCollider = playerAnimator.GetComponentsInChildren<DamageGeneric>();
        uiManager = GetComponent<UIManager>();
        HitCountTex.text = clicksCnt.ToString();
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

        if (currentAttackTime > 1f && clicksCnt > 0)
        {
            StartCoroutine(ResetHitCountAfterDelay(1f));
        }
    }

    public void OnLightAttackBtnPressed()
    {
        if (!playerGamePlayManager.isPlayingAnotherAnimation)
        {
            playerGamePlayManager.isPlayingAnotherAnimation = true;
            if (canHitLightAttack())
            {
                if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
                {
                    playerGamePlayManager.isLightAttack = true;
                    playerGamePlayManager.isHeavyAttack = false;
                    Debug.Log("----" + clicksCnt);
                    HitCountTex.text = " Hits - " + clicksCnt.ToString();
                    HitCountTex.gameObject.SetActive(true);
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
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
    }

    public void OnHeavyAttackBtnPressed()
    {
        if (!playerGamePlayManager.isPlayingAnotherAnimation)
        {
            playerGamePlayManager.isPlayingAnotherAnimation = true;
            if (canHitHeavyAttack())
            {
                if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
                {
                    playerGamePlayManager.isHeavyAttack = true;
                    playerGamePlayManager.isLightAttack = false;
                   
                    Debug.Log("----" + clicksCnt);
                    HitCountTex.text = " Hits - " + clicksCnt.ToString();
                    HitCountTex.gameObject.SetActive(true);
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
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
    }

    public void OnSpecialAttackBtnPressed()
    {
        playerGamePlayManager.isHeavyAttack = false;
        playerGamePlayManager.isLightAttack = false;

        if (!playerGamePlayManager.isPlayingAnotherAnimation)
        {
            
            if (canHitSpecialAttack())
            {
                playerGamePlayManager.isPlayingAnotherAnimation = true;
                playerGamePlayManager.isSpecialAttack = true;
                playerAnimator.SetTrigger("isSpecialAttack");
                weaponCollider[1].gameObject.SetActive(true);
                if (playerGamePlayManager.enemyGamePlayManager.enemyAIDecision.IsPlayerInAttackRange())
                    playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x - 1f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
                else
                    playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x - 1f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);

                uiManager.specialAttackBtnAnim.SetActive(false);
                SuperPowetText.gameObject.SetActive(false);
                clicksCnt = 0;
                playerGamePlayManager.isPlayingAnotherAnimation = false;
                StartCoroutine(SpecialAttackBuffer());
                StopCoroutine(SpecialAttackBuffer());
            }
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
    }

    IEnumerator SpecialAttackBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        //playerGamePlayManager.isSpecialAttack = false;
        ScoreManager.Instance.UpdatePlayerScore("isSpecialAttack");
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
        uiManager.specialAttackBtnAnim.SetActive(true);
        SuperPowetText.gameObject.SetActive(true);
        if (ScoreManager.Instance.characterStaminaValuePlayer >= remainingStamina && clicksCnt >= 3 && !playerGamePlayManager.isSpecialAttack)
        {
            
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

    IEnumerator ResetHitCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       /* if (clicksCnt > 3)
        {
            // Reset the HitCount after the delay
            clicksCnt = 0;
            HitCountTex.text = " Hits - " + clicksCnt.ToString();
            
        }*/
        StartCoroutine(DisableHitCountTextAfterDelay(0.5f));
    }

    IEnumerator DisableHitCountTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable the TMP_Text component or set the text to an empty string
        HitCountTex.gameObject.SetActive(false);
    }

}