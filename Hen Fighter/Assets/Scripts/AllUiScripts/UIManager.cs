using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject specialAttackBtnAnim;

    //For specialAttackButton Filling
    [SerializeField] RectTransform Holder;
    [SerializeField] Image CircleImage;
    [SerializeField] TMP_Text TxtProgress;

    [SerializeField] [Range(0, 1)] float progress = 0f;

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
        CircleImage.fillAmount = progress;
        TxtProgress.text = Mathf.Floor(progress * 100).ToString();
        Holder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));
    }

    public void SceneChange(string ScreneName)
	{
		SceneManager.LoadScene(ScreneName);
	}

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
