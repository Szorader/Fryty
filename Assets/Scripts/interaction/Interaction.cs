using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        return true;
    }
    
    public string GetPrompt()
    {
        return "E - Interact";
    }
}
