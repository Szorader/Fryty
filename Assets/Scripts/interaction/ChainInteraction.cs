using UnityEngine;

public class ChainInteraction : MonoBehaviour, IInteractable
{
    public string prompt;

    public QueueManager queueManager;

    public QueueType queueType; // wybierasz w Inspectorze

    public enum QueueType
    {
        Order,
        Pickup
    }
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        ClientController client = null;

        // wybór kolejki
        if (queueType == QueueType.Order)
        {
            client = queueManager.MoveFirstClientFromQueue(queueManager.orderQueue);
        }
        else if (queueType == QueueType.Pickup)
        {
            client = queueManager.MoveFirstClientFromQueue(queueManager.pickupQueue);
        }

        if (client == null)
        {
            Debug.Log("Brak klienta");
            return false;
        }

        // "zabicie" klienta
        queueManager.RemoveClient(client);

        // sprawdzanie czy zły czy dobry
        if (client.clientData.isBadClient)
        {
            Debug.Log("Udane! Zabiłeś złego klienta");
            queueManager.dayManager.GoodClient();
            queueManager.basket.UpdateMoney(15f);
        }
        else
        {
            Debug.Log("Zabiłeś dobrego klienta — koniec dnia");
            queueManager.dayManager.WrongClient();
            
            
            // tutaj możesz wywołać np.
            // GameManager.EndDay();
        }

        return true;
    }

    public string GetPrompt()
    {
        return prompt;
    }
}