using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;

public class ChannelManager : MonoBehaviour
{
    [Header("Channel Creation Settings")]
    [SerializeField] public GameObject channelPrefab;
    [SerializeField] public float channelSpacing = 0.18327776f; // Distance between channels in parent's local Z axis
    [SerializeField] public AudioManager audioManager;
    [SerializeField] private Vector3 startPosition = new Vector3(-0.604000032f, 2.27277899f, 6.02698708f);
    [SerializeField] private TextMeshPro channelNamePrefab;

    // Adding the initial offset as a constant to ensure consistency
    private const float INITIAL_Z_OFFSET = - 0.18f;

    [Header("Available Drum Samples")]
    [SerializeField] public List<DrumSampleData> availableSamples;
    private List<Channel> activeChannels = new List<Channel>();

    [System.Serializable]
    public class DrumSampleData
    {
        public string name;
        public AudioClip audioClip;
        public GameObject audioSourcePrefab;
    }

    public void CreateNewChannel(int sampleIndex)
    {
        if (sampleIndex < 0 || sampleIndex >= availableSamples.Count)
        {
            Debug.LogError("Invalid sample index!");
            return;
        }

        // Convert start position to local space if it's in world space
        Vector3 localStartPos = transform.InverseTransformPoint(startPosition);

        // Get the local position of the last channel or use local start position
        Vector3 lastLocalPosition = activeChannels.Count > 0 && activeChannels[activeChannels.Count - 1] != null
            ? activeChannels[activeChannels.Count - 1].transform.localPosition
            : localStartPos;

        // Calculate new position in local space
        Vector3 newLocalPosition;
        if (activeChannels.Count == 0)
        {
            // For the first channel, apply the initial offset
            newLocalPosition = new Vector3(
                lastLocalPosition.x,
                lastLocalPosition.y,
                lastLocalPosition.z - INITIAL_Z_OFFSET
            );
        }
        else
        {
            // For subsequent channels, use regular spacing
            newLocalPosition = new Vector3(
                lastLocalPosition.x,
                lastLocalPosition.y,
                lastLocalPosition.z - channelSpacing
            );
        }

        // Create the channel as a child of this manager, using local position
        GameObject newChannelObj = Instantiate(channelPrefab, transform);
        newChannelObj.transform.localPosition = newLocalPosition;
        newChannelObj.transform.localRotation = Quaternion.identity;

        Channel newChannel = newChannelObj.GetComponent<Channel>();
        if (newChannel != null)
        {
            DrumSampleData sampleData = availableSamples[sampleIndex];
            ConfigureChannel(newChannel, sampleData);
            activeChannels.Add(newChannel);

            if (audioManager != null)
            {
                audioManager.channels.Add(newChannel);
            }
        }
    }

    private void ConfigureChannel(Channel channel, DrumSampleData sampleData)
    {
        channel.channelName = sampleData.name;

        GameObject nameDisplay = new GameObject($"{sampleData.name}_Label");
        TextMeshPro tmpText = nameDisplay.AddComponent<TextMeshPro>();
        tmpText.text = sampleData.name;
        tmpText.fontSize = 1.4f;
        tmpText.alignment = TextAlignmentOptions.Left;
        nameDisplay.transform.SetParent(channel.transform);
        nameDisplay.transform.localPosition = new Vector3(8, 0, -0.2f);
        nameDisplay.transform.localRotation = Quaternion.Euler(90, 0, 0);

        var serializedObject = new UnityEditor.SerializedObject(channel);
        serializedObject.FindProperty("drumSample").objectReferenceValue = sampleData.audioClip;
        serializedObject.FindProperty("audioSourcePrefab").objectReferenceValue = sampleData.audioSourcePrefab;
        serializedObject.ApplyModifiedProperties();
    }

    public void RemoveChannel(Channel channel)
    {
        if (channel != null)
        {
            if (audioManager != null)
            {
                audioManager.channels.Remove(channel);
            }
            activeChannels.Remove(channel);
            Destroy(channel.gameObject);
            ReorganizeChannels();
        }
    }

    private void ReorganizeChannels()
    {
        if (activeChannels.Count == 0) return;

        // Convert start position to local space
        Vector3 localStartPos = transform.InverseTransformPoint(startPosition);

        // Get reference position from first channel or use local start position
        Vector3 referenceLocalPos = activeChannels[0] != null
            ? activeChannels[0].transform.localPosition
            : localStartPos;

        // Reposition all channels in local space
        for (int i = 0; i < activeChannels.Count; i++)
        {
            if (activeChannels[i] != null)
            {
                float zOffset = (i == 0) ? INITIAL_Z_OFFSET : INITIAL_Z_OFFSET + (channelSpacing * (i));
                Vector3 newLocalPos = new Vector3(
                    referenceLocalPos.x,
                    referenceLocalPos.y,
                    localStartPos.z - zOffset
                );
                activeChannels[i].transform.localPosition = newLocalPos;
            }
        }
    }
}