using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip Clip;

    // Start is called before the first frame update
    void Start()
    {
        Source.PlayOneShot(Clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
