using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Allows this class to be editable in the Unity Inspector
public class Sound
{
    public string name;
    public int[] steps = new int[16]; // 16-step sequence (0 = off, 1 = on)
    public AudioClip clip; // The actual sound file
}

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>(); // List of all sounds (Kick, Snare, etc.)
    public GameObject soundPrefab; // Prefab that contains an AudioSource
    public Transform spawnPoint; // Where sounds are instantiated
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
            foreach (var sound in sounds)
            {
                if (sound.steps[currentStep] == 1) // If this step is active
                {
                    PlaySound(sound.clip);
                }
            }

            currentStep = (currentStep + 1) % 16; // Move to the next step
            yield return new WaitForSeconds(stepDuration);
        }
    }

    void PlaySound(AudioClip clip)
    {
        GameObject soundObject = Instantiate(soundPrefab, spawnPoint.position, Quaternion.identity);
        SampleManager sampleManager = soundObject.GetComponent<SampleManager>();
        sampleManager.PlaySound(clip);
    }
}
