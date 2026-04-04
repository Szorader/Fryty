using System;
using UnityEngine;
using TMPro;

public class ClientInteraction : MonoBehaviour, IInteractable
{
    public QueueManager queueManager;
    
    private bool _canTakeOrder = true;
    private bool _canPickOrder = false;
    
    public TMP_Text promptText;

    private void Start()
    {
        queueManager = FindObjectOfType<QueueManager>();
        
        UpdatePrompt();
    }
    

    public bool CanInteract()
    {
            return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (_canTakeOrder)
        {
            queueManager.TakeOrder();
            _canTakeOrder = false;
            _canPickOrder = true;
            UpdatePrompt();
        }

        else if (_canPickOrder)
        {
            queueManager.ServeNextClient();
            _canPickOrder = false;
        }
        return true;
    }

    private void UpdatePrompt()
    {
        if (promptText != null)
        {
            if (_canTakeOrder)
                promptText.text = "E - Take order";
            if (_canPickOrder)
                promptText.text = "E - Pick Up Order";
        }
    }
}
