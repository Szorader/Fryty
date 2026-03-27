using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        return true;
    }
}
