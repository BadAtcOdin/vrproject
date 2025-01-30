using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public string name;
    public int[] steps; // Active steps (1-16)
    public GameObject sourcePrefab; // Prefab containing AudioSource
}

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>(); // List of all sounds (Kick, Snare, etc.)
    public Transform spawnPoint; // Where to instantiate sounds

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
            // Loop through each sound (Kick, Snare, etc.)
            foreach (var sound in sounds)
            {
                if (sound.steps[currentStep] == 1) // If this step is active for this sound
                {
                    Instantiate(sound.sourcePrefab, spawnPoint.position, Quaternion.identity);
                }
            }

            currentStep = (currentStep + 1) % 16; // Move to the next step
            yield return new WaitForSeconds(stepDuration);
        }
    }
}
