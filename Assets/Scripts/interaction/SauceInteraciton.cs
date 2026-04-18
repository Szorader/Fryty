using UnityEngine;

public class SauceInteraciton : MonoBehaviour, IInteractable
{
    public string prompt;
    private BasketData basketData;

    void Start()
    {
        basketData = GetComponentInParent<BasketData>();
    }
    public bool CanInteract()
    {
        return basketData.sauceType != OrderDatabase.SauceType.None;
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