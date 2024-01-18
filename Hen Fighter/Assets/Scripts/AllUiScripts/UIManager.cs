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


   // public GameObject DoYouWantToExitGame;
    public GameObject settingPaneel;
    
    //For specialAttackButton Animation
    [SerializeField] RectTransform Holder;
    [SerializeField] Image CircleImage;
    [SerializeField] TMP_Text TxtProgress;
    [SerializeField] [Range(0, 1)] float progress = 0f;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void SceneChange(string ScreneName)
	{
        Time.timeScale = 1f;

        
            SceneManager.LoadScene(ScreneName);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
