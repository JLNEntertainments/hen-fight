using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    [SerializeField] GameObject GameOverGameobject;


    static int enemyScore, playerScore;
    [SerializeField]private Image EnemyStaminaBarImage,PlayerStaminaBarImage;

    public float LightAttackDamage;
    public float HeavyAttackDamage;

    float staminaRegenRate, defaultStaminRegenRate;

    public TMP_Text Scoretext,ScoreTextDisplayOnHealth;
    [SerializeField]
    private TMP_Text EnemyHealthBarText,PlayerHealthBarText;

    [HideInInspector]
    public float maxStamina;
    public float characterStaminaValueEnemy, characterStaminaValuePlayer;


    private int HealthBarValue;

    public float StaminaBarCharingRate;
    public Coroutine StaminaBarRecharge;

    void Start()
    {
        defaultStaminRegenRate = 2f;
        staminaRegenRate = defaultStaminRegenRate;

        maxStamina = 1;
        characterStaminaValueEnemy = maxStamina;
        characterStaminaValuePlayer = maxStamina;
        enemyScore = 0;
        playerScore = 0;
    }

    private void Update()
    {
        staminaRegenRate += Time.deltaTime;
        if (staminaRegenRate > defaultStaminRegenRate)
        {
            RegenerateStamina();
            staminaRegenRate = 0;
        }
            
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
            EnemyStaminaBarImage.fillAmount = EnemyStaminaBarImage.fillAmount - (HealthBarValue * 0.01f);
        }
        Debug.Log("Enemy : " + enemyScore);
        Scoretext.text =playerScore.ToString();
        ScoreTextDisplayOnHealth.text = playerScore.ToString();


        EnemyHealthBarText.text = HealthBarValue.ToString();
      
    }

    public void UpdatePlayerScore(string attackType)
    {
        if (attackType.Equals("isLight"))
        {
            playerScore += 20;
            characterStaminaValuePlayer -= (characterStaminaValuePlayer * LightAttackDamage);
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }

        else if (attackType.Equals("isHeavy"))
        {
            playerScore += 40;
            characterStaminaValuePlayer -= (characterStaminaValuePlayer * HeavyAttackDamage);
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
        Debug.Log("Player : " + playerScore);
        Scoretext.text = playerScore.ToString();
        ScoreTextDisplayOnHealth.text = playerScore.ToString();
        EnemyHealthBarText.text = characterStaminaValueEnemy.ToString();
    }

    void RegenerateStamina()
    {
        if(characterStaminaValueEnemy < maxStamina)
        {
            characterStaminaValueEnemy += StaminaBarCharingRate / 10f;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;
        }

        if(characterStaminaValuePlayer < maxStamina)
        {
            characterStaminaValuePlayer += StaminaBarCharingRate / 10f;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
    }

    public void ShowGameOverPanel()
    {
        GameOverGameobject.SetActive(true);
    }
}
