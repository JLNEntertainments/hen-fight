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

    [HideInInspector]
    public DamageGeneric[] weaponCollider;
    public CapsuleCollider playerCapsuleCollider;
    public bool isAttacking;

    public TMP_Text HitCountTex;

    [HideInInspector]
    public float currentAttackTime, defaultAttackTime, remainingStamina;

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
        playerCapsuleCollider = playerAnimator.GetComponentInChildren<CapsuleCollider>();
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

        
    }

    public void OnLightAttackBtnPressed()
    {
        
    }

    public void OnHeavyAttackBtnPressed()
    {
       
    }

    public void OnSpecialAttackBtnPressed()
    {
       
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

    public void PlayAttackAnimation(bool heavyAttack, bool lightAttack)
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