/*using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject clientPrefab;
    public Transform spawnPoint;

    public ClientData[] clients;
    public OrderData[] orders;

    public QueueManager queueManager;

    public void SpawnClient()
    {
        GameObject obj = Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);

        ClientData clientData = clients[Random.Range(0, clients.Length)];
        OrderData orderData = orders[Random.Range(0, orders.Length)];

        Client clientInformation = new Client(clientData, orderData);

        ClientController controller = obj.GetComponent<ClientController>();
        controller.SetClient(clientInformation);

        queueManager.AddToOrderQueue(controller);
    }
}*/
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;
    public OrderData[] orders;

    public QueueManager queueManager;

    public void SpawnClient()
    {
        // 🎲 LOSOWANIE DANYCH KLIENTA
        ClientData clientData = clients[Random.Range(0, clients.Length)];

        // 🎲 LOSOWANIE ZAMÓWIENIA
        OrderData orderData = orders[Random.Range(0, orders.Length)];

        // 🧍 SPAWN PREFABU Z CLIENTSO
        GameObject obj = Instantiate(clientData.clientPrefab, spawnPoint.position, Quaternion.identity);

        // 📦 TWORZENIE DANYCH
        Client clientInformation = new Client(clientData, orderData);

        // 🔗 PODPIĘCIE DO CONTROLLERA
        ClientController controller = obj.GetComponent<ClientController>();
        controller.SetClient(clientInformation);

        // 📋 DODANIE DO KOLEJKI
        queueManager.AddToOrderQueue(controller);
    }
}