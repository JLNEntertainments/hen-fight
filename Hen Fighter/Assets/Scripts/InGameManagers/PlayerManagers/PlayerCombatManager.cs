using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonGeneric<PlayerCombatManager>
{
    Animator playerAnimator;
    PlayerGamePlayManager playerGamePlayManager;
    UIManager uiManager;

    static int clicksCnt;

    [SerializeField]
    DamageGeneric[] weaponCollider;
    public bool isAttacking;

    float currentAttackTime, defaultAttackTime, remainingStamina, lightAttackBuffer, heavyAttackBuffer, specialAttackBuffer, blockAttackBuffer;
    WaitForSeconds lightBuffer, heavyBuffer, spBuffer, blockBuffer;

    int assignmentCnt, randomLightAttack;

    //private AudioSource ClawSound;

   // private ParticleSystem particleForPlayer;

    void Start()
    {
        assignmentCnt = 0;
     // ClawSound  = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        //particleForPlayer = GameObject.FindGameObjectsWithTag("Particles").GetComponent<par>();
        
    }

    public void AssignplayerAttributes()
    {
       
            playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>();
            playerAnimator = playerGamePlayManager.GetComponent<Animator>();
            weaponCollider = playerAnimator.GetComponentsInChildren<DamageGeneric>();
            uiManager = GetComponent<UIManager>();

            clicksCnt = 0;
            defaultAttackTime = 2f;
            currentAttackTime = defaultAttackTime;
            lightAttackBuffer = heavyAttackBuffer = 0.8f;
            specialAttackBuffer = 4f;
            blockAttackBuffer = 1f;

            lightBuffer = new WaitForSeconds(lightAttackBuffer);
            heavyBuffer = new WaitForSeconds(heavyAttackBuffer);
            spBuffer = new WaitForSeconds(specialAttackBuffer);
            blockBuffer = new WaitForSeconds(blockAttackBuffer);

            TurnOffAttackpoints();
    }

    void Update()
    { 
        randomLightAttack = Random.Range(0, 2);
        currentAttackTime += Time.deltaTime;

        if (clicksCnt >= 3)
            canHitSpecialAttack();
    }

    public void OnLightAttackBtnPressed()
    {
        if (!playerGamePlayManager.canPerformCombat)
        {
            playerGamePlayManager.canPerformCombat = true;
            if (!playerGamePlayManager.isPlayingAnotherAnimation)
            {
                playerGamePlayManager.isPlayingAnotherAnimation = true;
                StartCoroutine(LightAttack());
                StopCoroutine(LightAttack());
                playerGamePlayManager.isPlayingAnotherAnimation = false;
            }
            playerGamePlayManager.canPerformCombat = false;
        }
    }

    IEnumerator LightAttack()
    {
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            isAttacking = true;
            playerGamePlayManager.isLightAttack = true;
            playerGamePlayManager.isHeavyAttack = false;
            clicksCnt++;
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
           // ClawSound.Play();
           
            currentAttackTime = 0;
            yield return lightBuffer;
            playerGamePlayManager.SetDefaultAnimationState();
            isAttacking = false;
            //TurnOffAttackpoints();
            
        }
    }

    public void OnHeavyAttackBtnPressed()
    {
        if (!playerGamePlayManager.canPerformCombat)
        {
            playerGamePlayManager.canPerformCombat = true;
            if (!playerGamePlayManager.isPlayingAnotherAnimation)
            {
                playerGamePlayManager.isPlayingAnotherAnimation = true;
                StartCoroutine(HeavyAttack());
                StopCoroutine(HeavyAttack());
                playerGamePlayManager.isPlayingAnotherAnimation = false;
            }
            playerGamePlayManager.canPerformCombat = false;
        }
    }

    IEnumerator HeavyAttack()
    {
        if (currentAttackTime > defaultAttackTime && !playerGamePlayManager.isTakingDamage)
        {
            isAttacking = true;
            playerGamePlayManager.isHeavyAttack = true;
            playerGamePlayManager.isLightAttack = false;
            clicksCnt++;
            PlayAttackAnimation(playerGamePlayManager.isHeavyAttack, playerGamePlayManager.isLightAttack);
           // ClawSound.Play();
            
            currentAttackTime = 0;
            yield return heavyBuffer;
            playerGamePlayManager.SetDefaultAnimationState();
            playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.transform.position.x + 1.85f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
            isAttacking = false;
            //TurnOffAttackpoints();
        }
    }

    public void OnSpecialAttackBtnPressed()
    {
        if (!playerGamePlayManager.canPerformCombat)
        {
            playerGamePlayManager.canPerformCombat = true;
            if (!playerGamePlayManager.isPlayingAnotherAnimation)
            {
                playerGamePlayManager.isPlayingAnotherAnimation = true;
                playerGamePlayManager.isTakingDamage = true;
                StartCoroutine(SpecialAttack());
                StopCoroutine(SpecialAttack());
                playerGamePlayManager.isTakingDamage = false;
                playerGamePlayManager.isPlayingAnotherAnimation = false;
            }
            playerGamePlayManager.canPerformCombat = false;
        }
    }

    IEnumerator SpecialAttack()
    {
        if (canHitSpecialAttack())
        {
            isAttacking = true;
            playerGamePlayManager.isSpecialAttack = true;
            playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_SPECIALATTACK);
            playerGamePlayManager.transform.position = new Vector3(playerGamePlayManager.enemyGamePlayManager.transform.position.x - 1.2f, playerGamePlayManager.transform.position.y, playerGamePlayManager.transform.position.z);
            yield return spBuffer;
            playerGamePlayManager.SetDefaultAnimationState();
            isAttacking = false;
            uiManager.specialAttackBtnAnim.SetActive(false);
            clicksCnt = 0;
            playerGamePlayManager.isSpecialAttack = false;
        }
    }

    public void OnBlockAttackBtnPressed()
    {
        StartCoroutine(BlockAttack());
        StopCoroutine(BlockAttack());
    }

    IEnumerator BlockAttack()
    {
        if(!playerGamePlayManager.isPlayingAnotherAnimation)
        {
            playerGamePlayManager.isPlayingAnotherAnimation = true;
            playerGamePlayManager.isBlocking = true;
            playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_BLOCK);
            yield return blockBuffer;
            playerGamePlayManager.SetDefaultAnimationState();
            playerGamePlayManager.isBlocking = false;
            playerGamePlayManager.isPlayingAnotherAnimation = false;
        }
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
        foreach (var obj in weaponCollider)
        {
            if (lightAttack && obj.gameObject.CompareTag("Beak"))
            {
                if (randomLightAttack == 0)
                {
                    playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_LIGHTATTACK);
                    obj.gameObject.SetActive(true);
                    return;
                }
                else if(randomLightAttack == 1)
                {
                    playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_LIGHTATTACKTOP);
                    obj.gameObject.SetActive(true);
                    return;
                }
            }
            else if (heavyAttack && obj.gameObject.CompareTag("Foot"))
            {
                obj.gameObject.SetActive(true);
                playerGamePlayManager.ChangeAnimationState(playerGamePlayManager.PLAYER_HEAVYATTACK);
                return;
            }
        }
    }
}