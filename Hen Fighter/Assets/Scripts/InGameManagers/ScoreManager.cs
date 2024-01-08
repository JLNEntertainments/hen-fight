using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    [SerializeField] GameObject YouLostPanel,YouWonpanel,GameOverpanel,TestGameObject;

    static int enemyScore, playerScore;
    [SerializeField]private Image EnemyStaminaBarImage,PlayerStaminaBarImage;

    public float enemyHealth;
    public float LightAttackDamage;
    public float HeavyAttackDamage;
    public float SpecialAttackDamage;

    float staminaRegenRate, defaultStaminRegenRate;

    public TMP_Text ScoretextForPlayer,ScoreTextForEnemy,ScoreDisplayOnGameOverPanelForPlayer;
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
        defaultStaminRegenRate = 1f;
        staminaRegenRate = defaultStaminRegenRate;

        enemyHealth = 1.0f;

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
            characterStaminaValueEnemy -= LightAttackDamage;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;

            enemyScore += 20;
        }
        else if (attackType.Equals("isHeavy"))
        {
            enemyScore += 40;
            characterStaminaValueEnemy -= HeavyAttackDamage;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;
            EnemyStaminaBarImage.fillAmount = EnemyStaminaBarImage.fillAmount - (HealthBarValue * 0.01f);
        }

        else if (attackType.Equals("isSpecialReact"))
        {
            enemyScore += 100;
            characterStaminaValueEnemy -= HeavyAttackDamage;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;
            EnemyStaminaBarImage.fillAmount = EnemyStaminaBarImage.fillAmount - (HealthBarValue * 0.01f);
        }
        Debug.Log("Enemy : " + enemyScore);
        //score for player
        ScoretextForPlayer.text =playerScore.ToString();
        ScoreDisplayOnGameOverPanelForPlayer.text = playerScore.ToString();
        ScoreTextForEnemy.text = enemyScore.ToString();
        PlayerHealthBarText.text = Mathf.Max(0, Mathf.RoundToInt(PlayerCombatManager.Instance.playerGamePlayManager.playerHealth * 100 )).ToString() + "%";
      
    }

    public void UpdatePlayerScore(string attackType)
    {
        if (attackType.Equals("isLight"))
        {
            playerScore += 20;
            characterStaminaValuePlayer -= LightAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }

        else if (attackType.Equals("isHeavy"))
        {
            playerScore += 40;
            characterStaminaValuePlayer -= HeavyAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
        else if (attackType.Equals("isSpecialAttack"))
        {
            playerScore += 100;
            characterStaminaValuePlayer -= SpecialAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }


        Debug.Log("Player : " + playerScore);
        //score for palyer
        ScoretextForPlayer.text = playerScore.ToString();
        ScoreDisplayOnGameOverPanelForPlayer.text = playerScore.ToString();
        ScoreTextForEnemy.text = enemyScore.ToString();
        EnemyHealthBarText.text = Mathf.Max(0,Mathf.RoundToInt ( ScoreManager.Instance.enemyHealth * 100)).ToString() + "%" ;
       


    }

    void RegenerateStamina()
    {
        if(characterStaminaValueEnemy < maxStamina)
        {
            characterStaminaValueEnemy += StaminaBarCharingRate / 10f;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;

        if (characterStaminaValueEnemy >= 0.276 && characterStaminaValueEnemy < 0.5)
        {
                EnemyStaminaBarImage.color = Color.red;
        }
       
       
        else 
        {
                EnemyStaminaBarImage.color = Color.yellow;
        }
    }



        if (characterStaminaValuePlayer < maxStamina)
        {
            characterStaminaValuePlayer += StaminaBarCharingRate / 10f;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
        if (characterStaminaValuePlayer >= 0.276 && characterStaminaValuePlayer < 0.5)
        {
            PlayerStaminaBarImage.color = Color.red;
        }
       
        else
        {
            PlayerStaminaBarImage.color = Color.yellow;

        }
    }

    public void ShowGameOverPanel()
    {
        YouLostPanel.SetActive(true);
        StartCoroutine(GameOverPanelDisplay());
        StopCoroutine(GameOverPanelDisplay());
        
    }

    public void TestGamonejctShow()
    {
        StartCoroutine(testGameobject());
        StopCoroutine(testGameobject());
    }

    IEnumerator GameOverPanelDisplay()
    {
        yield return new WaitForSeconds(2f);
        GameOverpanel.SetActive(true);
        // Time.timeScale = 0f;
    }
    
    public void ShowYouWonpanel()
    {
        YouWonpanel.SetActive(true);
        StartCoroutine(GameOverPanelDisplay());
        StopCoroutine(GameOverPanelDisplay());
    }

    IEnumerator testGameobject()
    {
        yield return new WaitForSeconds(3f);
        TestGameObject.SetActive(true);

        Time.timeScale = 0f;
    }
}
