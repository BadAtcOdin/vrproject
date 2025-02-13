using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Channel> channels = new List<Channel>();
    public Transform spawnPoint;
    public int bpm = 120;

    private double nextStepTime; // Using double for more precise timing
    private double sampleRate; // Audio sample rate
    private double stepDuration;
    private int currentStep = 0;
    private Dictionary<Channel, GameObject> activeAudioObjects = new Dictionary<Channel, GameObject>();

    private const double SCHEDULE_AHEAD_TIME = 0.1; // Schedule audio 100ms ahead
    private const double UPDATE_RATE = 0.03; // Update scheduling every 30ms

    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
        stepDuration = 60.0 / bpm / 4; // 4 steps per beat
        nextStepTime = AudioSettings.dspTime + 0.1; // Start slightly in the future
        StartCoroutine(ScheduleSteps());
    }

    IEnumerator ScheduleSteps()
    {
        while (true)
        {
            // Schedule audio events until the ahead time boundary
            double currentTime = AudioSettings.dspTime;
            while (nextStepTime < currentTime + SCHEDULE_AHEAD_TIME)
            {
                ScheduleStep(nextStepTime);
                nextStepTime += stepDuration;
            }

            yield return new WaitForSeconds((float)UPDATE_RATE);
        }
    }

    private void ScheduleStep(double time)
    {
        foreach (var channel in channels)
        {
            if (channel.activeSteps[currentStep] == 1)
            {
                PlayScheduledSound(channel, time);
            }
        }
        currentStep = (currentStep + 1) % 16;
    }

    private void PlayScheduledSound(Channel channel, double time)
    {
        // Stop previous sound if it exists
        if (activeAudioObjects.ContainsKey(channel))
        {
            GameObject previousAudio = activeAudioObjects[channel];
            if (previousAudio != null)
            {
                AudioSource previousSource = previousAudio.GetComponent<AudioSource>();
                if (previousSource != null)
                {
                    // Schedule the stop of the previous sound just before the new one
                    previousSource.SetScheduledEndTime(time);
                }
                // Schedule destruction slightly after the sound stops
                Destroy(previousAudio, (float)(time - AudioSettings.dspTime + 0.1f));
            }
        }

        // Create and schedule the new sound
        GameObject audioObject = Instantiate(channel.GetAudioSourcePrefab(), spawnPoint.position, Quaternion.identity);
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = channel.GetDrumSample();
            audioSource.PlayScheduled(time); // Use precise scheduling

            activeAudioObjects[channel] = audioObject;

            // Schedule cleanup after the clip finishes
            double clipDuration = (double)audioSource.clip.samples / audioSource.clip.frequency;
            StartCoroutine(RemoveFromActiveSoundsAfterPlay(channel, audioObject, clipDuration, time));
        }
    }

    private IEnumerator RemoveFromActiveSoundsAfterPlay(Channel channel, GameObject obj, double clipDuration, double startTime)
    {
        // Wait until after the scheduled time plus the clip duration
        double waitTime = (startTime + clipDuration) - AudioSettings.dspTime;
        yield return new WaitForSeconds((float)waitTime);

        if (activeAudioObjects.ContainsKey(channel) && activeAudioObjects[channel] == obj)
        {
            activeAudioObjects.Remove(channel);
        }
    }
}