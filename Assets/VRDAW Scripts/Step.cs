using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Step : MonoBehaviour
{
    public bool isActive = false;
    private StepManager stepManager;
    private XRBaseInteractable interactible;

    [SerializeField] private GameObject visualObject;
    [SerializeField] private Material baseMaterial;

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
                if (baseMaterial != null)
                {
                    stepMaterial = new Material(baseMaterial);
                    visualRenderer.material = stepMaterial;
                }
                else
                {
                    Debug.LogError($"{gameObject.name}: Base material is missing!");
                }
            }
            else
            {
                Debug.LogError($"{gameObject.name}: No Renderer found on Visual Object!");
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name}: Visual Object is NOT assigned!");
        }

        UpdateStepColor();
    }

    public void ToggleStep(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isActive = !isActive;
            stepManager.ToggleStep(isActive);

            UpdateStepColor();
        }
    }

    private void UpdateStepColor()
    {
        if (stepMaterial != null)
        {
            stepMaterial.color = isActive ? Color.green : Color.red;
        }
    }
}
