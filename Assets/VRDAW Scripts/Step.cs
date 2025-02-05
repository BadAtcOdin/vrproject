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
        
    }


    public void ToggleStep()
    {
        isActive = !isActive;
        Debug.Log("Pad is Toggled") ; 
        
    }
}

