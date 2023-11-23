using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen_orintation_landscape : MonoBehaviour
{
   
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
}
