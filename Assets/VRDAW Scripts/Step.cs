using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Step : MonoBehaviour
{
    public bool isActive = false;  // Is the step ON or OFF?
    private StepManager stepManager;
    private XRBaseInteractable interactible;

    [SerializeField] private GameObject visualObject;  // Assign this in the prefab
    [SerializeField] private Material baseMaterial;    // Assign a base material in Unity Inspector

    private Renderer visualRenderer;
    private Material stepMaterial;

    void Start()
    {
        interactible = GetComponent<XRBaseInteractable>();
        interactible.hoverEntered.AddListener(ToggleStep);
        stepManager = GetComponentInParent<StepManager>();

        if (visualObject != null)
        {
            visualRenderer = visualObject.GetComponent<Renderer>();

            if (visualRenderer != null)
            {
                // Create a unique material instance for this step using the base material
                if (baseMaterial != null)
                {
                    stepMaterial = new Material(baseMaterial);
                    visualRenderer.material = stepMaterial;
                    Debug.Log($"{gameObject.name}: Unique material assigned.");
                }
                else
                {
                    Debug.LogError($"{gameObject.name}: Base material is missing! Assign one in the Inspector.");
                }
            }
            else
            {
                Debug.LogError($"{gameObject.name}: No Renderer found on Visual Object!", visualObject);
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name}: Visual Object is NOT assigned!", gameObject);
        }

        UpdateStepColor();  // Initialize the color
    }

    public void ToggleStep(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isActive = !isActive;
            stepManager.isToggled = isActive;

            Debug.Log($"{gameObject.name}: Toggled to {(isActive ? "ON" : "OFF")}");
            UpdateStepColor();
        }
    }

    private void UpdateStepColor()
    {
        if (stepMaterial != null)
        {
            stepMaterial.color = isActive ? Color.green : Color.red;
            Debug.Log($"{gameObject.name}: Color changed to {(isActive ? "Green" : "Red")}");
        }
        else
        {
            Debug.LogError($"{gameObject.name}: stepMaterial is NULL!");
        }
    }
}
