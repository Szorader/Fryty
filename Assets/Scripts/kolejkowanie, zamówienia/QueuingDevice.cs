using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class QueuingDevice : MonoBehaviour
{

    //ustawianie w kolejce
    public Transform orderStartPoint;
    public Transform pickPoint;
    public Transform exitPoint;
    public float spaceBetweenClients = 1.5f;
    
    public List<ClientController> pickList = new List<ClientController>();
    public Queue<ClientController> orderQueue = new Queue<ClientController>();
    public List<WaitingPoint>  waitingPoints = new List<WaitingPoint>();


    private SpawnOrderTicket spawnOrderTicket;
    private SpawnManager spawnManager;
    public BasketInteraction basket;
    public DayManager dayManager;
    public Tutorial tutorial;
    public UI_QueuingDevice uiQueuingDevice;
    


    public bool canGiveNumber = false;
    private int countClients = 0;
    public int currentNumber;
    public bool waitingForTake = false;
    private bool doOnce = true;
    
    

    private void Awake()
    {
        spawnOrderTicket = FindObjectOfType<SpawnOrderTicket>();
        spawnManager = FindObjectOfType<SpawnManager>();
        tutorial = FindObjectOfType<Tutorial>();
        uiQueuingDevice = FindObjectOfType<UI_QueuingDevice>();
        if (tutorial == null)
        {
            basket = FindObjectOfType<BasketInteraction>();
        }
        
        dayManager = FindObjectOfType<DayManager>();
        
        //wypelnia liste nullami
        while (pickList.Count <= spawnManager.clientsOfTheDay)
        {
            pickList.Add(null);
        }
    }

    private void Update()
    {
        if (countClients == spawnManager.clientsOfTheDay && doOnce)
        {
            dayManager.timeToClean = true;
            dayManager.CleanTime();
            doOnce = false;
        }
        
    }
    //nadanie numeru pikacza
    public void GiveNumber(int number)
    {
        
        //Debug.Log(number);
        
        canGiveNumber = false;
        spawnManager.currentClients--;
        
        //zmiana listy
        ClientController client = orderQueue.Dequeue();
        pickList[number] = client;
        client.PikPikNumber = number;
        
    //idzie sobie gdzies czekac
        WaitingPoint point = GetRandomFreePoint();
        if (point != null)
        {
            point.isReserved = true;
            Vector3 position = point.transform.position;
            client.point = point;
            
            client.MoveTo(position);
        }
        UpdatePositionOrderQueue();

    }

    public void AddToOrderQueue(ClientController client)
    {
        orderQueue.Enqueue(client);
        UpdatePositionOrderQueue();
    }

    public void UpdatePositionOrderQueue()
    {
        Vector3 currentPosition = orderStartPoint.position;

        foreach (var client in orderQueue)
        {
            client.MoveTo(currentPosition);
            currentPosition.z -= spaceBetweenClients;
        }
    }

    public void TakeOrder()
    {
        ClientController client = orderQueue.Peek();
        spawnOrderTicket.SpawnTicket(client.customerOrder);
        client.Toggle();
        canGiveNumber = true;
    }
    
    public WaitingPoint GetRandomFreePoint()
    {
        
        List<WaitingPoint> freePoints = waitingPoints.FindAll(p => !p.isReserved);
        
        if (freePoints.Count == 0)
        {
            Debug.LogWarning("Brak wolnych punktów!");
            return null;
        }

        return freePoints[UnityEngine.Random.Range(0, freePoints.Count)];
        
    }
    /// <summary>
    /// przywoływanie klienta do odbioru
    /// </summary>
    public void AddOrderToBasket(int number)
    {
        if (pickList.Count == 0)
            return;
        
        
        //jezeli klikniemy w przywolonie i nie jest przypisany klient to return
        ClientController client = pickList[number];
        if (client == null)
            return;
        else
            uiQueuingDevice.canGiveOrder = false;
        
        basket.currentCustomer = client.customerOrder;
        basket.waitingTime = client.waitingTime;
        basket.satisfaction = client.satisfaction;
        basket.isBad = client.isBadClient;

        client.MoveTo(pickPoint.position);
        client.point.isReserved = false;
        
        currentNumber = number;
        waitingForTake = true;
        
        
    }
    
    /// <summary>
    /// odchodzi juz z zamówieniem
    /// </summary>
    public void RemoveClient()
    {
        ClientController client = pickList[currentNumber];
        client.MoveTo(exitPoint.position);
        StartCoroutine(ExitRoutine(client));
        
    }
    
    IEnumerator ExitRoutine(ClientController client)
    {
        yield return new WaitForSeconds(2f);
        Destroy(client.gameObject);
        countClients++;
        waitingForTake = false;
    }
    
    public void KillClient(ClientController client)
    {
        Destroy(client.gameObject);
        countClients++;
        StartCoroutine(Wait(3));
    }

    IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        UpdatePositionOrderQueue();
    }
}