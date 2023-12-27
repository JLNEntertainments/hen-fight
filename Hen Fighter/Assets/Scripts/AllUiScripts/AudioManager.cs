using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string audioTag = "Audio"; // Set your desired tag in the Inspector
    public AudioClip[] audioClips;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomAudio();
    }

   public void PlayRandomAudio()
    {
        // Filter audio clips based on the tag
        AudioClip[] filteredClips = System.Array.FindAll(audioClips, clip => clip.name.StartsWith(audioTag));

        if (filteredClips.Length > 0)
        {
            // Select a random audio clip
            AudioClip randomClip = filteredClips[Random.Range(0, filteredClips.Length)];

            // Play the selected audio clip
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clips with the specified tag found.");
        }
    }
}
