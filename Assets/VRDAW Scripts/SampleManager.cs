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
}
