using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class BPMSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform sliderHandle;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private TextMeshPro bpmText;
    [SerializeField] private AudioManager audioManager;

    [Header("Slider Settings")]
    [SerializeField] private float minBPM = 60f;
    [SerializeField] private float maxBPM = 200f;

    // Reference to the grab interactable component
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Vector3 initialHandlePosition;
    private bool isGrabbed = false;
    private float sliderLength;

    void Start()
    {
        // Store initial position and calculate slider length
        initialHandlePosition = sliderHandle.position;
        sliderLength = Vector3.Distance(startPoint.position, endPoint.position);

        // Set up the grab interactable
        grabInteractable = sliderHandle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            // Configure the grab interactable for slider behavior
            grabInteractable.trackPosition = true;
            grabInteractable.trackRotation = false;
            grabInteractable.throwOnDetach = false;

            // Add listeners for grab events
            grabInteractable.selectEntered.AddListener(OnGrabStart);
            grabInteractable.selectExited.AddListener(OnGrabEnd);
        }

        // Initialize BPM display
        UpdateBPMDisplay(audioManager.bpm);
    }

    void Update()
    {
        if (isGrabbed)
        {
            // Calculate the slider position along the track
            Vector3 handlePos = sliderHandle.position;
            Vector3 sliderDirection = (endPoint.position - startPoint.position).normalized;
            Vector3 relativePosition = handlePos - startPoint.position;

            // Project the handle position onto the slider direction
            float sliderValue = Vector3.Dot(relativePosition, sliderDirection) / sliderLength;
            sliderValue = Mathf.Clamp01(sliderValue);

            // Convert slider value to BPM
            int newBPM = Mathf.RoundToInt(Mathf.Lerp(minBPM, maxBPM, sliderValue));

            // Update the audio manager and display
            if (audioManager.bpm != newBPM)
            {
                audioManager.SetBPM(newBPM);
                UpdateBPMDisplay(newBPM);
            }

            // Constrain handle movement to the slider track
            Vector3 constrainedPosition = startPoint.position + sliderDirection * (sliderValue * sliderLength);
            sliderHandle.position = constrainedPosition;
        }
    }

    private void OnGrabStart(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    private void UpdateBPMDisplay(int bpm)
    {
        if (bpmText != null)
        {
            bpmText.text = $"{bpm} BPM";
        }
    }
}