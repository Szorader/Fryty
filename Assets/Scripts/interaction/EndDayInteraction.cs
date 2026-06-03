using UnityEngine;
using FMODUnity;


public class EndDayInteraction : MonoBehaviour, IInteractable
{
    // go next day, skip or end current day
    private DayManager dayManager;
    private TrashManager trashManager;

    public bool clicked = false;
    
    public string prompt;

    public bool isTutorial = false;
    
    //Audio
    [SerializeField] private EventReference driveAway;

    void Start()
    {
        dayManager = FindObjectOfType<DayManager>();
        trashManager = FindObjectOfType<TrashManager>();
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
        if(!isTutorial)
            if (!trashManager.clean)
                return false;
        
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
