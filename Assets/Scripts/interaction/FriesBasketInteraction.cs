using System;
using UnityEngine;

public class FriesBasketInteraction : MonoBehaviour, IInteractable
{
    private FryerSystem fryerSystem;
    public string prompt;

    
   
    
     void Awake()
    {
        fryerSystem = GetComponentInParent<FryerSystem>();
    }

    public bool CanInteract()
    {
        return fryerSystem != null && fryerSystem.hasFries && !fryerSystem.returning;
    }

    public bool Interact(Interactor interactor)
    {
        if (!CanInteract()) return false;
        
        fryerSystem.OnMouseDown();
        return true;
    }
    
    public string GetPrompt()
    {
        return prompt;
    }
}
