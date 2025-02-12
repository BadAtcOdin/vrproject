using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SampleManager : MonoBehaviour
{
    private AudioSource audioSource;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length); // Destroy after sound plays
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(gameObject, clip.length); // Destroy after sound plays
    }
}
