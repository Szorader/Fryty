using UnityEngine;
using FMODUnity;

public class EndDayInteraction : MonoBehaviour, IInteractable
{
    // go next day, skip or end current day
    private DayManager dayManager;

    public bool clicked = false;
    
    public string prompt;
    
    //Audio
    [SerializeField] private EventReference driveAway;

    void Start()
    {
        dayManager = FindObjectOfType<DayManager>();
    }
    public bool CanInteract()
    {
        if (!clicked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Interact(Interactor interactor)
    {
        RuntimeManager.PlayOneShot(driveAway);
        
        // go next day, skip or skip current day
        dayManager.EndDay();
        clicked = true;
        return true;
    }

    public string GetPrompt()
    {
        return prompt;
    }
}
