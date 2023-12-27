using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    
    public TMP_Text loadingText;
    public Slider loadingSlider;
    public string sceneToLoad;

    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        loadingSlider.value = 0;
         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;
        float progress = 0;
        while (!asyncLoad.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime);
            loadingSlider.value = progress;
            if(progress >= 0.9f)
            {
                loadingSlider.value = 1;
                loadingText.text = progress * 100 + "%";
                asyncLoad.allowSceneActivation = true;
            }

             yield return null;
        }
    }
}
