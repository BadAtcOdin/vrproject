using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PadOnOff : MonoBehaviour
{
    private XRBaseInteractable interactable ;
    // Start is called before the first frame update
    private bool isActive ;
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        // interactable.hoverEntered.AddListener(TogglePad);
    }

    /* public void TogglePad(BaseInteractionEventArgs hover)
    {
        if(hover.interactableObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject ;
            isActive = !isActive ;  
        }

    } */

    // Update is called once per frame
    void Update()
    {
        
    }
}
