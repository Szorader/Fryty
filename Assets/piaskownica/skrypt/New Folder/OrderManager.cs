using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public QueueManager queueManager;

    public void TakeOrder()
    {
        ClientController client = queueManager.GetFirstOrderClient();

        if (client == null)
        {
            Debug.Log("Brak klientów");
            return;
        }

        Debug.Log("Klient: " + client.clientData.data.clientName);
        Debug.Log("Zamówienie: " + client.clientData.order.orderName);

        foreach (string ingredient in client.clientData.order.ingredients)
        {
            Debug.Log("- " + ingredient);
        }

        // przeniesienie do kolejki odbioru
        queueManager.AddToPickupQueue(client);
    }
}