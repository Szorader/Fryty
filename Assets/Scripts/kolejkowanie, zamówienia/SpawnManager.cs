/*using UnityEngine;

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
}*/

using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;
    public string[] clientNames;

    public QueueManager queueManager;

    [Header("Spawn Settings")]
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1f;

    public int maxClients = 3;
    public int currentClients = 0;
    public int clientsOfTheDay = 5;
    public int clientsCount = 0;

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
        if (clientsCount == clientsOfTheDay)
        {
            Debug.Log("end day");
            return;
        }
        ClientData clientData = clients[Random.Range(0, clients.Length)];
        string randomName = clientNames[Random.Range(0, clientNames.Length)];
        
        GameObject obj = Instantiate(clientData.clientPrefab, spawnPoint.position, Quaternion.identity);
        
        ClientController controller = obj.GetComponent<ClientController>();
        controller.SetClient(clientData, randomName);

        queueManager.AddToQueue(controller, queueManager.orderQueue);

        currentClients++;
        clientsCount++;
    }
}
