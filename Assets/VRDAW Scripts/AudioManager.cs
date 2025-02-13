using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Channel> channels = new List<Channel>(); // Reference to all Channels
    public Transform spawnPoint;

    public int bpm = 120;
    private float stepDuration;
    private int currentStep = 0;

    void Start()
    {
        stepDuration = 60f / bpm / 4; // 4 steps per beat
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        while (true)
        {
            foreach (var channel in channels)
            {
                if (channel.activeSteps[currentStep] == 1)
                {
                    PlaySound(channel);
                }
            }

            currentStep = (currentStep + 1) % 16; // Move to the next step
            yield return new WaitForSeconds(stepDuration);
        }
    }

    private void PlaySound(Channel channel)
    {
        GameObject audioObject = Instantiate(channel.GetAudioSourcePrefab(), spawnPoint.position, Quaternion.identity);
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.clip = channel.GetDrumSample();
            audioSource.Play();
            Destroy(audioObject, audioSource.clip.length);
        }
    }
}
