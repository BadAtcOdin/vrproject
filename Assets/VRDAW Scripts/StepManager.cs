using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StepManager : MonoBehaviour
{
    public int index; // Step index (0-15)
    public bool isToggled = false;
    private Channel parentChannel; // Reference to the parent channel

    void Start()
    {
        parentChannel = GetComponentInParent<Channel>();
        if (parentChannel == null)
        {
            Debug.LogError($"{gameObject.name}: No parent Channel found!");
        }
    }

    public void ToggleStep(bool active)
    {
        isToggled = active;

        if (parentChannel != null)
        {
            parentChannel.UpdateStepState(index, active);
        }
    }
}
