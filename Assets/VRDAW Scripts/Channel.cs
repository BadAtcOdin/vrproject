using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    public GameObject stepPrefab;
    public int stepCount = 16;
    public float stepSpacing = 0.25f;
    public string channelName; // "Kick", "Snare", etc.

    public List<Step> steps = new List<Step>();
    public int[] activeSteps = new int[16];

    [SerializeField] private GameObject audioSourcePrefab; // Prefab with AudioSource
    [SerializeField] private AudioClip drumSample; // Assign in the Inspector

    void Start()
    {
        SetupSteps();

        // Initialize activeSteps array
        for (int i = 0; i < 16; i++)
        {
            activeSteps[i] = 0;
        }
    }

    public void UpdateStepState(int index, bool isActive)
    {
        activeSteps[index] = isActive ? 1 : 0;
    }

    public GameObject GetAudioSourcePrefab()
    {
        return audioSourcePrefab;
    }

    public AudioClip GetDrumSample()
    {
        return drumSample;
    }


    void SetupSteps()
    {
        StepManager[] existingSteps = GetComponentsInChildren<StepManager>();

        if (existingSteps.Length > 0)
        {
            // If steps exist, just update their indices
            for (int i = 0; i < existingSteps.Length; i++)
            {
                existingSteps[i].index = i ;
            }
        }
        else
        {
            // Create new steps if none exist
            CreateSteps();
        }
    }

    void CreateSteps()
    {
        // Ensure we get the Drumrack's transform (parent of Channel)
        Transform drumrackTransform = transform.parent;
        Quaternion drumrackRotation = drumrackTransform != null ? drumrackTransform.rotation : Quaternion.identity;

        for (int i = 0; i < stepCount; i++)
        {
            // Calculate position along the X-axis
            Vector3 stepPosition = transform.position + new Vector3(i * stepSpacing, 0, 0);

            // Instantiate step with correct rotation
            GameObject newStep = Instantiate(stepPrefab, stepPosition, drumrackRotation, transform);

            // Assign the step index
            StepManager stepManager = newStep.GetComponent<StepManager>();
            if (stepManager != null)
            {
                stepManager.index = i ;
            }
        }
    }
}
