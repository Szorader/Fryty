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
    
    
    [Range(0f, 1f)]
    public float badClientChance = 0.25f;

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

    /*public void SpawnClient()
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
    }*/
    public void SpawnClient()
    {
        if (clientsCount == clientsOfTheDay)
        {
            Debug.Log("end day");
            return;
        }

        ClientData clientData = clients[Random.Range(0, clients.Length)];
        string randomName = clientNames[Random.Range(0, clientNames.Length)];

        // 25% szans na złego klienta
        bool isBad = Random.value <= 0.25f;
        
        GameObject obj = Instantiate(clientData.clientPrefab, spawnPoint.position, Quaternion.identity);
        
        
        // Pobieramy renderer
        Renderer renderer = obj.GetComponentInChildren<MeshRenderer>();

        if (renderer != null)
        {
            Material chosenMaterial = null;

            if (isBad && clientData.badMaterials.Length > 0)
            {
                chosenMaterial = clientData.badMaterials[Random.Range(0, clientData.badMaterials.Length)];
            }
            else if (!isBad && clientData.goodMaterials.Length > 0)
            {
                chosenMaterial = clientData.goodMaterials[Random.Range(0, clientData.goodMaterials.Length)];
            }

            if (chosenMaterial != null)
            {
                // NIE robimy new Material() jeśli nie musisz
                renderer.material = chosenMaterial;
            }
        }

        ClientController controller = obj.GetComponent<ClientController>();

        // przekazujemy info czy zły
        controller.SetClient(clientData, randomName, isBad);

        queueManager.AddToQueue(controller, queueManager.orderQueue);

        currentClients++;
        clientsCount++;
    }
}
