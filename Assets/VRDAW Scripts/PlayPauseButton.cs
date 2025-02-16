using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class PlayPauseButton : MonoBehaviour
{
    [SerializeField] private GameObject visualObject;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private TextMeshPro buttonLabel;

    private bool isPlaying = false;
    private XRBaseInteractable interactible;
    private AudioManager audioManager;
    private Renderer visualRenderer;
    private Material buttonMaterial;

    void Start()
    {
        // Set up XR interaction
        interactible = GetComponent<XRBaseInteractable>();
        interactible.hoverEntered.AddListener(TogglePlayState);

        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError($"{gameObject.name}: No AudioManager found in scene!");
        }

        // Set up visual components
        InitializeVisuals();
        UpdateButtonState();
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
            else
            {
                Debug.LogError($"{gameObject.name}: Missing renderer or base material!");
            }
        }
    }

    public void TogglePlayState(BaseInteractionEventArgs hover)
    {
        // Only respond to poke interactions
        if (hover.interactorObject is XRPokeInteractor)
        {
            isPlaying = !isPlaying;
            if (audioManager != null)
            {
                audioManager.SetPlaybackState(isPlaying);
            }
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        // Update visual feedback
        if (buttonMaterial != null)
        {
            buttonMaterial.color = isPlaying ? Color.green : Color.red;
        }

        // Update button label
        if (buttonLabel != null)
        {
            buttonLabel.text = isPlaying ? "PAUSE" : "PLAY";
        }
    }
}