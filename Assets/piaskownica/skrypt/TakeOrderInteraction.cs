using System;
using UnityEngine;

public class TakeOrderInteraction : MonoBehaviour, IInteractable
{
    public OrderManager orderManager;
    private bool _canTakeOrder = true;

    private void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }
    

    public bool CanInteract()
    {
        if (_canTakeOrder)
            return true;
        else
            return false;
            
    }

    public bool Interact(Interactor interactor)
    {
        if (_canTakeOrder)
        {
            orderManager.TakeOrder();
        }
        _canTakeOrder = false;

        return true;
    }
}
