using System.Collections;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;
    public string[] clientNames;

    private QueuingDevice queuingDevice;

    [Header("Tutorial Spawn Settings")]
    public bool canSpawn = false;

    private int spawnedClients = 0;
    private int maxTutorialClients = 2;

    [Range(0f, 1f)]
    public float badClientChance = 1f; // zawsze zły klient do tutorialu

    void Start()
    {
        queuingDevice = FindObjectOfType<QueuingDevice>();
        // Spawn pierwszego klienta od razu
        SpawnClient();
    }

    void Update()
    {
        // Jeśli można spawnąć kolejnego
        // i jeszcze nie osiągnęliśmy limitu 2 klientów
        if (canSpawn && spawnedClients < maxTutorialClients)
        {
            canSpawn = false;
            SpawnClient();
        }
    }

    public void SpawnClient()
    {
        // zabezpieczenie
        if (spawnedClients >= maxTutorialClients)
        {
            Debug.Log("Tutorial clients finished");
            return;
        }

        ClientData clientData = clients[Random.Range(0, clients.Length)];
        string randomName = clientNames[Random.Range(0, clientNames.Length)];

        // drugi klient = zły klient
        bool isBad = spawnedClients == 1;

        GameObject obj = Instantiate(
            clientData.clientPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Renderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

        if (renderer != null)
        {
            Material chosenMaterial = null;

            if (isBad && clientData.badMaterials.Length > 0)
            {
                chosenMaterial = clientData.badMaterials[
                    Random.Range(0, clientData.badMaterials.Length)
                ];
            }
            else if (!isBad && clientData.goodMaterials.Length > 0)
            {
                chosenMaterial = clientData.goodMaterials[
                    Random.Range(0, clientData.goodMaterials.Length)
                ];
            }

            if (chosenMaterial != null)
            {
                renderer.material = chosenMaterial;
            }
        }

        ClientController controller = obj.GetComponent<ClientController>();

        controller.SetClient(clientData, randomName, isBad);

        queuingDevice.AddToOrderQueue(controller);

        spawnedClients++;

        Debug.Log($"Spawned client: {spawnedClients} | Bad: {isBad}");
    }
}