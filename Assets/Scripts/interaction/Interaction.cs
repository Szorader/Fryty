using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
    public string prompt;
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("dkfjdlhasjfdshakjh");
        return true;
    }
    
    public string GetPrompt()
    {
        return prompt;
    }
}
