using UnityEngine;

public class CutterInteraction : MonoBehaviour, IInteractable
{
    public string prompt;
    private Cutter cutter;
    
    void Start()
    {
        cutter = GetComponentInParent<Cutter>();
        
    }
    public bool CanInteract()
    {
        return cutter.hasPotatoLoaded && !cutter.isProcessing;
    }

    public bool Interact(Interactor interactor)
    {
        
        //return true;
        return false;
    }
    
    public string GetPrompt()
    {
        return prompt;
    }

    
}
