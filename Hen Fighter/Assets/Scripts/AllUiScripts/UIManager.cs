using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject specialAttackBtnAnim;

    [SerializeField]
    Animator[] EnemyFXAnim;
    [SerializeField]
    Animator[] PlayerFXAnim;

    public GameObject DoYouWantToExitGame;
    
    //For specialAttackButton Filling
    [SerializeField] RectTransform Holder;
    [SerializeField] Image CircleImage;
    [SerializeField] TMP_Text TxtProgress;

    [SerializeField] [Range(0, 1)] float progress = 0f;

    private void Awake()
    {
        /*if (GameObject.FindWithTag("Enemy") != null)
            EnemyFXAnim = GameObject.FindWithTag("Enemy").GetComponentsInChildren<Animator>();

        PlayerFXAnim = GameObject.FindWithTag("Player").GetComponentsInChildren<Animator>();*/
    }

    public void AssignCharacters(GameObject enemyClone, GameObject playerClone)
    { 
        EnemyFXAnim = enemyClone.gameObject.GetComponentsInChildren<Animator>();
        PlayerFXAnim = playerClone.gameObject.GetComponentsInChildren<Animator>();

        TurnOffEnemyFXObjects();
        TurnOffPlayerFXObjects();
    }
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                DoYouWantToExitGame.SetActive(true);
                //Application.Quit();
                return;
            }
        }
        
    }
    void Update()
    {
        /*CircleImage.fillAmount = progress;
        TxtProgress.text = Mathf.Floor(progress * 100).ToString();
        Holder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));*/
    }

    void TurnOffEnemyFXObjects()
    {
        EnemyFXAnim[1].gameObject.SetActive(false);
        EnemyFXAnim[2].gameObject.SetActive(false);
    }

    void TurnOffPlayerFXObjects()
    {
        PlayerFXAnim[1].gameObject.SetActive(false);
        PlayerFXAnim[2].gameObject.SetActive(false);
    }

    public void SceneChange(string ScreneName)
	{
		SceneManager.LoadScene(ScreneName);
	}

    public void EnemyLightFX()
    {
        StartCoroutine(PlayEnemyLightFX());
        StopCoroutine(PlayEnemyLightFX());
    }

    IEnumerator PlayEnemyLightFX()
    {
        EnemyFXAnim[1].gameObject.SetActive(true);
        EnemyFXAnim[1].Play("EnemyBeakAttack");
        yield return new WaitForSeconds(0.3f);
        EnemyFXAnim[1].gameObject.SetActive(false);
    }

    public void EnemyHeavyFX()
    {
        StartCoroutine(PlayEnemyHeavyFX());
        StopCoroutine(PlayEnemyHeavyFX());
    }

    IEnumerator PlayEnemyHeavyFX()
    {
        EnemyFXAnim[2].gameObject.SetActive(true);
        EnemyFXAnim[2].Play("HeavyAttackAnim");
        yield return new WaitForSeconds(0.4f);
        EnemyFXAnim[2].gameObject.SetActive(false);
    }

    public void PlayerHeavyFX()
    {
        StartCoroutine(PlayPlayerHeavyFX());
        StopCoroutine(PlayPlayerHeavyFX());
    }

    IEnumerator PlayPlayerHeavyFX()
    {
        PlayerFXAnim[2].gameObject.SetActive(true);
        PlayerFXAnim[2].Play("HeavyAttackAnim");
        yield return new WaitForSeconds(0.4f);
        PlayerFXAnim[2].gameObject.SetActive(false);
    }

    public void PlayerLightFX()
    {
        StartCoroutine(PlayPlayerLightFX());
        StopCoroutine(PlayPlayerLightFX());
    }

    IEnumerator PlayPlayerLightFX()
    {
        yield return new WaitForSeconds(0.4f);
        PlayerFXAnim[1].gameObject.SetActive(true);
        PlayerFXAnim[1].Play("EnemyBeekAtack");
        yield return new WaitForSeconds(0.3f);
        PlayerFXAnim[1].gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
