using UnityEngine;
using TMPro;

/// <summary>
/// nowe kolejkowanie klientow przy uzyciu pikacza
/// </summary>
public class PikaczInteraction : MonoBehaviour, IInteractable
{
    public int index;
    public string prompt;

    public TMP_Text nameText;

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
        if (queuingDevice == null)
            return true;
        
        if (!queuingDevice.canGiveNumber)
            return true;
        
        if (queuingDevice.orderQueue == null || queuingDevice.orderQueue.Count == 0)
            return true;
        
        ClientController client = queuingDevice.orderQueue.Peek();
        
        if (client != null)
        {
            
            CustomerOrder order = client.GetComponent<CustomerOrder>();

            if (order != null && nameText != null)
            {
                nameText.text = order.clientName + ": #" + (index + 1);
            }
        }
        
        Debug.Log(index);
        queuingDevice.GiveNumber(index, this);
        gameObject.SetActive(false);
        //gameObject.SetActive(false);
        
        
        return true;
    }

    public string GetPrompt()
    {
        return prompt;
    }
}