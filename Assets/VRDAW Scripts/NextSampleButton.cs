using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NextSampleButton : MonoBehaviour
{
    [SerializeField] private GameObject visualObject;
    [SerializeField] private Material baseMaterial;
    private XRBaseInteractable interactible;
    private ChannelCreationUI channelUI;
    private Renderer visualRenderer;
    private Material buttonMaterial;

    void Start()
    {
        // Set up XR interaction
        interactible = GetComponent<XRBaseInteractable>();
        interactible.hoverEntered.AddListener(OnButtonPressed);

        // Find the ChannelCreationUI in the parent
        channelUI = GetComponentInParent<ChannelCreationUI>();
        if (channelUI == null)
        {
            Debug.LogError($"{gameObject.name}: No ChannelCreationUI found!");
        }

        InitializeVisuals();
    }

    private void InitializeVisuals()
    {
        if (visualObject != null)
        {
            visualRenderer = visualObject.GetComponent<Renderer>();
            if (visualRenderer != null && baseMaterial != null)
            {
                buttonMaterial = new Material(baseMaterial);
                visualRenderer.material = buttonMaterial;
            }
        }
    }

    public void OnButtonPressed(BaseInteractionEventArgs hover)
    {
        // Only respond to poke interactions
        if (hover.interactorObject is XRPokeInteractor)
        {
            channelUI?.NextSample();
        }
    }
}