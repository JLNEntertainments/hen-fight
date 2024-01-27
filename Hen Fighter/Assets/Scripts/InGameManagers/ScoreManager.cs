using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    [SerializeField] GameObject YouLostPanel,YouWonpanel,GameOverpanel,TestGameObject;

    UIManager uiManager;

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

    [SerializeField]
    private TMP_Text PlayerStaminaText, EnemyStaminaText;

    [HideInInspector]
    public float maxStamina = 1;
    public float characterStaminaValueEnemy, characterStaminaValuePlayer;
    public float health;
    private float lerpSpeed = 0.05f;
    

    private int HealthBarValue;

    public float StaminaBarCharingRate;
    public Coroutine StaminaBarRecharge;

    public TMP_Text damageTextPrefab;

    public TMP_Text OutOfStamina;

    public GameObject staminaBarBgAnimPlayer, staminaBarBgAnimEnemy;

    float damageValue = 0f;

    void Start()
    {
        defaultStaminRegenRate = 2.5f;
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
            characterStaminaValuePlayer = Mathf.Clamp01(characterStaminaValuePlayer);
            characterStaminaValueEnemy = Mathf.Clamp01(characterStaminaValueEnemy);
            PlayerStaminaText.text = Mathf.Max(0, Mathf.RoundToInt(characterStaminaValuePlayer * 100)).ToString() + "%";
            EnemyStaminaText.text = Mathf.Max(0, Mathf.RoundToInt(characterStaminaValueEnemy * 100)).ToString() + "%";
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
        //PlayerHealthBarText.text = Mathf.Max(0, Mathf.RoundToInt(PlayerCombatManager.Instance.playerGamePlayManager.playerHealth * 100 )).ToString() + "%";
        EnemyStaminaText.text = Mathf.Max(0, Mathf.RoundToInt(characterStaminaValueEnemy * 100)).ToString() + "%";


    }

    public void UpdatePlayerScore(string attackType)
    {
        if (attackType.Equals("isLight"))
        {
            playerScore += 20;
            characterStaminaValuePlayer -= LightAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
            damageValue = LightAttackDamage;


        }

        else if (attackType.Equals("isHeavy"))
        {
            playerScore += 40;
            characterStaminaValuePlayer -= HeavyAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
            damageValue = HeavyAttackDamage;

        }
        else if (attackType.Equals("isSpecialAttack"))
        {
            playerScore += 100;
            characterStaminaValuePlayer -= SpecialAttackDamage;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
            damageValue = SpecialAttackDamage;

        }
        Debug.Log("Player : " + playerScore);
        //score for palyer
        ScoretextForPlayer.text = playerScore.ToString();
        ScoreDisplayOnGameOverPanelForPlayer.text = playerScore.ToString();
        ScoreTextForEnemy.text = enemyScore.ToString();
        EnemyHealthBarText.text = Mathf.Max(0,Mathf.RoundToInt ( ScoreManager.Instance.enemyHealth * 100)).ToString() + "%" ;
        PlayerStaminaText.text = Mathf.Max(0, Mathf.RoundToInt(characterStaminaValuePlayer * 100)).ToString() + "%";


    }

    void RegenerateStamina()
    {
        if(characterStaminaValueEnemy < maxStamina)
        {
            characterStaminaValueEnemy += StaminaBarCharingRate / 10f;
            EnemyStaminaBarImage.fillAmount = characterStaminaValueEnemy;

        if (characterStaminaValueEnemy <= 0.24)
        {
                EnemyStaminaBarImage.color = Color.red;
                PlayerStaminaText.color = Color.white;
                staminaBarBgAnimEnemy.gameObject.SetActive(true);
            }
        else 
        {
                EnemyStaminaBarImage.color = Color.yellow;
                PlayerStaminaText.color = Color.red;
                staminaBarBgAnimEnemy.gameObject.SetActive(false);
            }
    }



        if (characterStaminaValuePlayer < maxStamina)
        {
            characterStaminaValuePlayer += StaminaBarCharingRate / 10f;
            PlayerStaminaBarImage.fillAmount = characterStaminaValuePlayer;
        }
        if (characterStaminaValuePlayer <= 0.24)
        {
            PlayerStaminaBarImage.color = Color.red;
            PlayerStaminaText.color = Color.white;
            OutOfStamina.gameObject.SetActive(true);
            staminaBarBgAnimPlayer.gameObject.SetActive(true);
        }
       
        else
        {
            PlayerStaminaBarImage.color = Color.yellow;
            PlayerStaminaText.color = Color.red;
            OutOfStamina.gameObject.SetActive(false);
            staminaBarBgAnimPlayer.gameObject.SetActive(false);

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
