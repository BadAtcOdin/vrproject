using UnityEngine;
using TMPro;

public class ChannelCreationUI : MonoBehaviour
{
    [SerializeField] public ChannelManager channelManager;
    [SerializeField] private TextMeshPro sampleNameText;
    [SerializeField] private int currentSampleIndex = 0;

    private void Start()
    {
        UpdateSampleNameDisplay();
    }

    public void NextSample()
    {
        currentSampleIndex++;
        if (currentSampleIndex >= channelManager.availableSamples.Count)
            currentSampleIndex = 0;
        UpdateSampleNameDisplay();
    }

    public void PreviousSample()
    {
        currentSampleIndex--;
        if (currentSampleIndex < 0)
            currentSampleIndex = channelManager.availableSamples.Count - 1;
        UpdateSampleNameDisplay();
    }

    public void CreateChannel()
    {
        if (channelManager == null)
        {
            Debug.LogError($"ChannelManager is null on {gameObject.name}! DrumRack reference may be missing.");
            return;
        }

        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError($"GameObject {gameObject.name} is inactive!");
            return;
        }

        channelManager.CreateNewChannel(currentSampleIndex);
    }

    private void UpdateSampleNameDisplay()
    {
        if (sampleNameText != null && channelManager != null)
        {
            var samples = channelManager.availableSamples;
            if (currentSampleIndex >= 0 && currentSampleIndex < samples.Count)
            {
                sampleNameText.text = samples[currentSampleIndex].name;
            }
        }
    }
}