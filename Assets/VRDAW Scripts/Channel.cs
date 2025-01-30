using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    public string channelName = "Kick"; // Default sound channel (Kick, Snare, etc.)

    void Start()
    {
        int stepIndex = 1;

        foreach (Transform child in transform)
        {
            Step stepScript = child.GetComponent<Step>();
            if (stepScript != null)
            {
                stepScript.stepIndex = stepIndex;
                stepIndex++;
            }
        }
    }
}
