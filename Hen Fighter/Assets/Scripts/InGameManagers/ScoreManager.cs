using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    static int enemyScore, playerScore;
    [SerializeField]private Image EnemyStaminaBarImage,PlayerStaminaBarImage;

    public float LightAttackDamage;
    public float HeavyAttackDamage;
   



    [HideInInspector]
    public float maxStamina;
    float characterStaminaValueEnemy, characterStaminaValuePlayer;

    void Start()
    {
       // EnemyStaminaBarImage.fillAmount = PlayerStaminaBarImage.fillAmount = 100f;
        maxStamina = 1;
        characterStaminaValueEnemy = maxStamina;
        characterStaminaValuePlayer = maxStamina;
        enemyScore = 0;
        playerScore = 0;
    }

    public void UpdateEnemyScore(string attackType)
    {
        if (attackType.Equals("isLight"))
        {
            characterStaminaValueEnemy -= (characterStaminaValueEnemy * LightAttackDamage);
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;
            enemyScore += 20;
        }

        else if (attackType.Equals("isHeavy"))
        {
            enemyScore += 40;
            characterStaminaValueEnemy -= (characterStaminaValueEnemy * HeavyAttackDamage);
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;
        }

            

        Debug.Log("Enemy : " + enemyScore);
    }



    public void UpdatePlayerScore(string attackType)
    {
        if (attackType.Equals("isLight"))
        {
            playerScore += 20;
            characterStaminaValuePlayer -= (characterStaminaValuePlayer * LightAttackDamage);
           // characterStaminaValuePlayer *= LightAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }

        else if (attackType.Equals("isHeavy"))
        {
            playerScore += 40;
            characterStaminaValuePlayer -= (characterStaminaValuePlayer * HeavyAttackDamage);
           // characterStaminaValuePlayer *= HeavyAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
        Debug.Log("Player : " + playerScore);
    }
}
