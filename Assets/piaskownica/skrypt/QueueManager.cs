using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Order Queue Points")]
    public Transform[] orderPoints;

    [Header("Pickup Queue Points")]
    public Transform[] pickupPoints;

    public List<ClientController> orderQueue = new List<ClientController>();
    public List<ClientController> pickupQueue = new List<ClientController>();

    // ================= ORDER QUEUE =================

    public void AddToOrderQueue(ClientController client)
    {
        orderQueue.Add(client);
        UpdateOrderQueuePositions();
    }

    public ClientController GetFirstOrderClient()
    {
        if (orderQueue.Count == 0) return null;

        ClientController client = orderQueue[0];
        orderQueue.RemoveAt(0);

        UpdateOrderQueuePositions();
        return client;
    }

    void UpdateOrderQueuePositions()
    {
        for (int i = 0; i < orderQueue.Count; i++)
        {
            if (i < orderPoints.Length)
                orderQueue[i].MoveTo(orderPoints[i].position);
        }
    }

    // ================= PICKUP QUEUE =================

    public void AddToPickupQueue(ClientController client)
    {
        pickupQueue.Add(client);
        UpdatePickupQueuePositions();
    }

    public ClientController GetFirstPickupClient()
    {
        if (pickupQueue.Count == 0) return null;

        ClientController client = pickupQueue[0];
        pickupQueue.RemoveAt(0);

        UpdatePickupQueuePositions();
        return client;
    }

    void UpdatePickupQueuePositions()
    {
        for (int i = 0; i < pickupQueue.Count; i++)
        {
            if (i < pickupPoints.Length)
                pickupQueue[i].MoveTo(pickupPoints[i].position);
        }
    }
}