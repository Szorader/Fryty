using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class QueueManager : MonoBehaviour
{
    public Transform exitPoint;
    
    public Transform orderStartPoints;
    public Transform pickupStartPoints;
    public float spaceBetweenClients = 1.5f;
    
    
    public Queue<ClientController> orderQueue = new Queue<ClientController>();
    public Queue<ClientController> pickupQueue = new Queue<ClientController>();


    public BasketInteraction basket;
    

    //queuing customers
    private void UpdateQueuePosition(Queue<ClientController> queue, Vector3 startPosition)
    {
        Vector3 currentPosition = startPosition;

        foreach (var client in queue)
        {
            client.MoveTo(currentPosition);
            currentPosition.z -= spaceBetweenClients;
        }
    }

    
    public void AddToQueue(ClientController client, Queue<ClientController> queue)
    {
        queue.Enqueue(client);
        
        Transform point = GetPoint(queue);
        UpdateQueuePosition(queue, point.position);
        
        if (queue == pickupQueue && queue.Count == 1)
        {
            AddOrderToBasket();
        }
    }

    
    public ClientController MoveFirstClientFromQueue(Queue<ClientController> queue)
    {
        if (queue.Count == 0) return null;
        
        ClientController client = queue.Dequeue();
        
        Transform point = GetPoint(queue);
        UpdateQueuePosition(queue, point.position);
        
        return client;
    }

    //getting point for start queue posiotion
    private Transform GetPoint(Queue<ClientController> queue)
    {
        Transform point = null;
        if (queue == orderQueue)
        {
            point = orderStartPoints;
        }
        else if (queue == pickupQueue)
        {
            point = pickupStartPoints; 
        }
        else
        {
            Debug.Log("Błąd przy definiowaniu startpoint");
        }
        return point;
    }


    public void AddOrderToBasket()
    {
        ClientController client = pickupQueue.Peek();
        basket.currentCustomer = client.customerOrder;
        basket.waitingTime = client.waitingTime;
        basket.satisfaction = client.satisfaction;
    }
    
    public void RemoveClient(ClientController client)
    {
        
        client.MoveTo(exitPoint.position);
        StartCoroutine(ExitRoutine(client));
        
        UpdateQueuePosition(orderQueue, orderStartPoints.position);
        
        UpdateQueuePosition(pickupQueue, pickupStartPoints.position);
    }

    IEnumerator ExitRoutine(ClientController client)
    {
        yield return new WaitForSeconds(2f);
        Destroy(client.gameObject);
    }
    
    public void TakeOrder()
    {

        ClientController client = MoveFirstClientFromQueue(orderQueue);
        
        if (client == null)
        {
            Debug.Log("Brak klientów");
            return;
        }
        AddToQueue(client, pickupQueue);

    }
    
    
    
    public void ServeNextClient()
    {
        ClientController client = MoveFirstClientFromQueue(pickupQueue);

        if (client == null)
        {
            Debug.Log("Brak klientów do obsługi");
            return;
        }
        
        RemoveClient(client);
        AddOrderToBasket();
    }
}