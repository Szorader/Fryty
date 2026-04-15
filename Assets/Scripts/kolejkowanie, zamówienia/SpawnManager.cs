using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;

    public QueueManager queueManager;

    public void SpawnClient()
    {
        ClientData clientData = clients[Random.Range(0, clients.Length)];
        
        GameObject obj = Instantiate(clientData.clientPrefab, spawnPoint.position, Quaternion.identity);
        
        
       ClientController controller = obj.GetComponent<ClientController>();
       controller.SetClient(clientData);
       
        queueManager.AddToQueue(controller, queueManager.orderQueue);
    }
}
/*
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;

    public QueueManager queueManager;

    [Header("Spawn Settings")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    public int maxClients = 3;
    private int currentClients = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (currentClients < maxClients)
            {
                SpawnClient();
            }
        }
    }

    public void SpawnClient()
    {
        ClientData clientData = clients[Random.Range(0, clients.Length)];
        
        GameObject obj = Instantiate(clientData.clientPrefab, spawnPoint.position, Quaternion.identity);
        
        ClientController controller = obj.GetComponent<ClientController>();
        controller.SetClient(clientData);

        queueManager.AddToQueue(controller, queueManager.orderQueue);

        currentClients++;
    }

    // TO MUSI BYĆ wywołane gdy klient odejdzie
    public void ClientLeft()
    {
        currentClients--;
    }
}
*/