using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCount : MonoBehaviour
{
    public TMP_Text fpsText;  // Reference to the Text component
    float deltaTime = 0.0f;

    void Update()
    {
       DisplayFps();
    }

    
    void DisplayFps()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Update the FPS text
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {fps:0.}";
    }
}


