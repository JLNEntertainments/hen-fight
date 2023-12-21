using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject specialAttackBtnAnim;
    //public Animation HitFX;
    public Animator HitFx,PlayerHitFx,heavyAttackForEnemy,heavyAttackForPlayer;
    
    //AnimationClip HitFX;
    //For specialAttackButton Filling
    [SerializeField] RectTransform Holder;
    [SerializeField] Image CircleImage;
    [SerializeField] TMP_Text TxtProgress;

    [SerializeField] [Range(0, 1)] float progress = 0f;

    public ParticleSystem particleForPlayer;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                // Insert Code Here (I.E. Load Scene, Etc)
                Application.Quit();
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

    public void SceneChange(string ScreneName)
	{
		SceneManager.LoadScene(ScreneName);
	}

    public void PlayFX()
    {
        StartCoroutine(PlayHitFX());
        StopCoroutine(PlayHitFX());
    }

    IEnumerator PlayHitFX()
    {
        HitFx.gameObject.SetActive(true);
        HitFx.Play("EnemyBeekAtack");
        particleForPlayer.Play();
        yield return new WaitForSeconds(0.3f);
        HitFx.gameObject.SetActive(false);
    }

    public void PlayEnemyhaveyAttack()
    {
        StartCoroutine(PalyEnemyHaveyFx());
        StopCoroutine(PalyEnemyHaveyFx());
    }

    IEnumerator PalyEnemyHaveyFx()
    {
        heavyAttackForEnemy.gameObject.SetActive(true);
        heavyAttackForEnemy.Play("HeavyAttack");
        particleForPlayer.Play();
        yield return new WaitForSeconds(0.3f);
        heavyAttackForEnemy.gameObject.SetActive(false);
    }

    public void PlayPlayerhaveyAttack()
    {
        StartCoroutine(PalyEnemyHaveyFx());
        StopCoroutine(PalyEnemyHaveyFx());
    }

    IEnumerator PalyPlayerHaveyFx()
    {
        heavyAttackForPlayer.gameObject.SetActive(true);
        heavyAttackForPlayer.Play("HeavyAttack");
        particleForPlayer.Play();
        yield return new WaitForSeconds(0.3f);
        heavyAttackForPlayer.gameObject.SetActive(false);
    }



    public void PlayerFX()
    {
        StartCoroutine(PlayerHitFX());
        StopCoroutine(PlayerHitFX());
    }

    IEnumerator PlayerHitFX()
    {
        PlayerHitFx.gameObject.SetActive(true);
        PlayerHitFx.Play("EnemyBeekAtack");
        particleForPlayer.Play();
        yield return new WaitForSeconds(0.2f);
        PlayerHitFx.gameObject.SetActive(false);
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
