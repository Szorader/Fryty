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