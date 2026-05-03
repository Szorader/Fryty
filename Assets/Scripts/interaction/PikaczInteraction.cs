using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// nowe kolejkowanie klientow przy uzyciu pikacza
/// </summary>
public class PikaczInteraction : MonoBehaviour, IInteractable
{
    public int index;
    public string prompt;
    private QueuingDevice queuingDevice;

    void Start()
    {
        queuingDevice = FindObjectOfType<QueuingDevice>();
    }
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (queuingDevice.canGiveNumber)
        {
            queuingDevice.GiveNumber(index);
            this.GameObject().SetActive(false);
        }
        
        return true;
    }
    
    public string GetPrompt()
    {
        return prompt;
    }
}
