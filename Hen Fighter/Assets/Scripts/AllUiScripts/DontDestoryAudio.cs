using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryAudio : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] musiObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if(musiObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
      //  DontDestroyOnLoad(this.gameObject);
    }
}
