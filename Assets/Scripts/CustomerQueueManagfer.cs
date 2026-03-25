using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueueManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private bool autoSpawn = true;
    [SerializeField] private int maxSpawnedCustomers = 10;

    [Header("Queue 1 - Waiting4Service")]
    [SerializeField] private Transform queue1StartPoint;

    [Header("Queue 2 - Waiting4Fries")]
    [SerializeField] private Transform queue2StartPoint;

    [Header("Exit")]
    [SerializeField] private Transform endPoint;

    [Header("Queue Spacing")]
    [SerializeField] private Vector3 queueOffset = new Vector3(0f, 0f, -1.5f);

    private Queue<CustomerQueueMember> queue1 = new Queue<CustomerQueueMember>();
    private Queue<CustomerQueueMember> queue2 = new Queue<CustomerQueueMember>();
    private List<CustomerQueueMember> allCustomers = new List<CustomerQueueMember>();

    private void Start()
    {
        if (autoSpawn)
            StartCoroutine(SpawnCustomersRoutine());
    }

    private IEnumerator SpawnCustomersRoutine()
    {
        while (true)
        {
            if (CanSpawnMoreCustomers())
                SpawnCustomer();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public bool CanSpawnMoreCustomers()
    {
        CleanNulls();
        return allCustomers.Count < maxSpawnedCustomers;
    }

    public void SpawnCustomer()
    {
        if (!CanSpawnMoreCustomers()) return;

        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        customerObj.tag = "Waiting4Service";

        CustomerQueueMember member = customerObj.GetComponent<CustomerQueueMember>();
        if (member == null) member = customerObj.AddComponent<CustomerQueueMember>();
        member.Initialize(this);

        if (!allCustomers.Contains(member))
            allCustomers.Add(member);

        EnqueueCustomer(member);
    }

    private void EnqueueCustomer(CustomerQueueMember member)
    {
        if (member == null) return;

        switch (member.gameObject.tag)
        {
            case "Waiting4Service":
                queue1.Enqueue(member);
                UpdateQueuePositions(queue1, queue1StartPoint);
                break;
            case "Waiting4Fries":
                queue2.Enqueue(member);
                UpdateQueuePositions(queue2, queue2StartPoint);
                break;
            case "HappyWithFries":
                SendCustomerToExit(member);
                break;
            case "Dead":
                DestroyCustomer(member);
                break;
        }
    }

    public void OnCustomerTagChanged(CustomerQueueMember member, string newTag)
    {
        if (member == null) return;
        
        RemoveFromQueue(member);
        
        EnqueueCustomer(member);
    }

    private void RemoveFromQueue(CustomerQueueMember member)
    {
        if (queue1.Contains(member))
        {
            Queue<CustomerQueueMember> temp = new Queue<CustomerQueueMember>();
            while (queue1.Count > 0)
            {
                var c = queue1.Dequeue();
                if (c != member) temp.Enqueue(c);
            }
            queue1 = temp;
            UpdateQueuePositions(queue1, queue1StartPoint);
        }

        if (queue2.Contains(member))
        {
            Queue<CustomerQueueMember> temp = new Queue<CustomerQueueMember>();
            while (queue2.Count > 0)
            {
                var c = queue2.Dequeue();
                if (c != member) temp.Enqueue(c);
            }
            queue2 = temp;
            UpdateQueuePositions(queue2, queue2StartPoint);
        }
    }

    private void SendCustomerToExit(CustomerQueueMember member)
    {
        if (member == null) return;

        if (endPoint == null)
        {
            DestroyCustomer(member);
            return;
        }

        member.GoToExit(endPoint.position);
    }

    public void NotifyCustomerReachedExit(CustomerQueueMember member)
    {
        DestroyCustomer(member);
    }

    private void DestroyCustomer(CustomerQueueMember member)
    {
        if (member == null) return;

        RemoveFromQueue(member);
        allCustomers.Remove(member);

        Destroy(member.gameObject);
    }

    private void UpdateQueuePositions(Queue<CustomerQueueMember> queue, Transform startPoint)
    {
        if (startPoint == null) return;

        int i = 0;
        foreach (var member in queue)
        {
            if (member == null) continue;
            Vector3 targetPos = startPoint.position + (queueOffset * i);
            member.SetTargetPosition(targetPos);
            i++;
        }
    }

    private void CleanNulls()
    {
        allCustomers.RemoveAll(x => x == null);
    }

    public int GetQueue1Count() => queue1.Count;
    public int GetQueue2Count() => queue2.Count;
    public int GetAllCustomersCount() => allCustomers.Count;
}