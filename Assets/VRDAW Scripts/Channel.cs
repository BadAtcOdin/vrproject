using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    public GameObject stepPrefab;  // Prefab for the Step object
    public int stepCount = 16;     // Number of steps
    public float stepSpacing = 0.25f;  // Distance between each step
    public string name = string.Empty;

    void Start()
    {
        SetupSteps();
    }

    void SetupSteps()
    {
        StepManager[] existingSteps = GetComponentsInChildren<StepManager>();

        if (existingSteps.Length > 0)
        {
            // If steps exist, just update their indices
            for (int i = 0; i < existingSteps.Length; i++)
            {
                existingSteps[i].index = i + 1;
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
                stepManager.index = i + 1;
            }
        }
    }
}
