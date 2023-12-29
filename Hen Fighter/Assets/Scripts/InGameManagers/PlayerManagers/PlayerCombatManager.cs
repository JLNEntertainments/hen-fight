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

    private AudioSource ClawSound;

    void Start()
    {
        ClawSound  = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }

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
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            playerGamePlayManager.isLightAttack = true;
            playerGamePlayManager.isHeavyAttack = false;
            clicksCnt++;
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
            ClawSound.Play();
            currentAttackTime = 0;
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
    }

    public void OnHeavyAttackBtnPressed()
    {
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            playerGamePlayManager.isHeavyAttack = true;
            playerGamePlayManager.isLightAttack = false;
            clicksCnt++;
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
            ClawSound.Play();
            currentAttackTime = 0;
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
    }

    public void OnSpecialAttackBtnPressed()
    {
        if (canHitSpecialAttack())
        {
            playerGamePlayManager.isSpecialAttack = true;
            playerAnimator.SetTrigger("isSpecialAttack");
            playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x + 1.2f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
            uiManager.specialAttackBtnAnim.SetActive(false);
            clicksCnt = 0;
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
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
        yield return new WaitForSeconds(0.4f);
        playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.transform.position.x + 1.2f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
        TurnOffAttackpoints();
    }
}