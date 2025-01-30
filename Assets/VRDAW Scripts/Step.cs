using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Step : MonoBehaviour
{
    public int stepIndex; // Step position (1-16)
    public bool isActive = false; // Is the pad ON or OFF?
    
    private Renderer rend; 

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateVisual();
    }

    void UpdateVisual()
    {
        // Change color based on activation state
        rend.material.color = isActive ? Color.green : Color.red;
    }

    public void ToggleStep()
    {
        isActive = !isActive;
        UpdateVisual();
    }
}
