using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown : MonoBehaviour
{
    float currentTime = 0f;
    [SerializeField] float startingTime = 100f;
    [SerializeField] GameObject gameoverImage;

    [SerializeField] TMP_Text CountTimeText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        CountTimeText.text = currentTime.ToString("0");
       
        if (currentTime <= 0)
        {
            currentTime = 0;
            gameoverImage.SetActive(true);
        }
        else
        if(currentTime <= 10)
        {
            CountTimeText.color = Color.red;
        }

    }
}
