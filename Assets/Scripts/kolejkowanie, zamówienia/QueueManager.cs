using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class QueueManager : MonoBehaviour
{
    public Transform exitPoint;

    [Header("Order Queue Points")]
    public Transform[] orderPoints;

    [Header("Pickup Queue Points")]
    public Transform[] pickupPoints;

    public Queue<ClientController> orderQueue = new Queue<ClientController>();
    public Queue<ClientController> pickupQueue = new Queue<ClientController>();
    
    public ServingBasket servingBasket;

    // ================= ORDER QUEUE =================

    public void AddToOrderQueue(ClientController client)
    {
        orderQueue.Enqueue(client);
        UpdateOrderQueuePositions();
    }

    public ClientController GetFirstOrderClient()
    {
        if (orderQueue.Count == 0) return null;

        ClientController client = orderQueue.Dequeue();
        UpdateOrderQueuePositions();
        return client;
    }

    void UpdateOrderQueuePositions()
    {
        int i = 0;
        foreach (var client in orderQueue)
        {
            if (i < orderPoints.Length)
                client.MoveTo(orderPoints[i].position);
            i++;
        }
    }

    // ================= PICKUP QUEUE =================

    public void AddToPickupQueue(ClientController client)
    {
        pickupQueue.Enqueue(client);
        UpdatePickupQueuePositions();
    }

    public ClientController GetFirstPickupClient()
    {
        if (pickupQueue.Count == 0) return null;

        ClientController client = pickupQueue.Dequeue();
        UpdatePickupQueuePositions();
        return client;
    }

    void UpdatePickupQueuePositions()
    {
        int i = 0;
        foreach (var client in pickupQueue)
        {
            if (i < pickupPoints.Length)
                client.MoveTo(pickupPoints[i].position);
            i++;
        }
    }

    public void RemoveClient(ClientController client)
    {
        //if (pickupQueue.Count == 0) return;
        
        client.MoveTo(exitPoint.position);
        StartCoroutine(ExitRoutine(client));
        
        // UWAGA: Queue nie ma Remove(x), więc trzeba przebudować kolejkę
        /*Queue<ClientController> newOrderQueue = new Queue<ClientController>();
        foreach (var c in orderQueue)
        {
            if (c != client)
                newOrderQueue.Enqueue(c);
        }

        orderQueue = newOrderQueue;*/

        UpdateOrderQueuePositions();
        UpdatePickupQueuePositions();
    }

    IEnumerator ExitRoutine(ClientController client)
    {
        yield return new WaitForSeconds(2f);
        Destroy(client.gameObject);
    }
    
    public void TakeOrder()
    {
      
        ClientController client = GetFirstOrderClient();
        
        if (client == null)
        {
            Debug.Log("Brak klientów");
            return;
        }

        Debug.Log("Klient: " + client.clientData.data.clientName);
        Debug.Log("Zamówienie: " + client.clientData.order.orderName);
        
      

        /*foreach (string ingredient in client.clientData.order.ingredients)
        {
            Debug.Log("- " + ingredient);
        }*/

        // przeniesienie do kolejki odbioru
        AddToPickupQueue(client);
    }
    
    
    
    public void ServeNextClient()
    {
        // pobierz pierwszego klienta z kolejki odbioru
        ClientController client = GetFirstPickupClient();

        if (client == null)
        {
            Debug.Log("Brak klientów do obsługi");
            return;
        }

        // wywołujemy tackę
        
        //tray.Serve(client);
        servingBasket.Serve(client);
        RemoveClient(client);
    }
}