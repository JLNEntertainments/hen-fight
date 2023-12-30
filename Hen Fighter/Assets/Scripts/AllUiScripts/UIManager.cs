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

    public GameObject DoYouWantToExitGame;
    
    //For specialAttackButton Animation
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
                DoYouWantToExitGame.SetActive(true);
                //Application.Quit();
                return;
            }
        }
        
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
