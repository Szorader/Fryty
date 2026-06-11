using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public ClientData[] clients;
    public string[] clientNames;

    //public QueueManager queueManager;
    public QueuingDevice queuingDevice;

    [Header("Spawn Settings")]
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1f;

    public int maxClients = 3;
    public int currentClients = 0;
    public int clientsOfTheDay = 2;
    public int clientsCount = 0;
    
    
    [Range(0f, 1f)]
    public float badClientChance = 0.1f;

    public bool isTutorial = false;

    void Start()
    {
        StartCoroutine(SpawnLoop());
        clientsOfTheDay = CalculateClientsOfTheDay(SaveSystem.Instance.saveData.day);
    }
    
    int CalculateClientsOfTheDay(int day)
    {
        float value =
            Mathf.Sqrt(day) +
            Mathf.Pow(2f, day / 20f) +
            1f;

        return Mathf.RoundToInt(value);
    }

    IEnumerator SpawnLoop()
    {
        while (!isTutorial)
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

    // Randomly decide if evil
    bool isBad = Random.value <= badClientChance;

    GameObject obj = Instantiate(
        clientData.clientPrefab,
        spawnPoint.position,
        Quaternion.identity
    );
    
    // if customer is evil -> enable growths via the RottenCustomerVisuals script
    RottenCustomerVisuals visuals = obj.GetComponent<RottenCustomerVisuals>();

    if (visuals != null)
    {
        if (isBad)
            visuals.SetupBadVisuals();
        else
            visuals.SetupGoodVisuals();
    }
    
    // Get renderer
    Renderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

    if (renderer != null)
    {
        Material chosenBodyMaterial = null;
        Material chosenFaceMaterial = null;

        // BODY MATERIAL (randomized)
        if (isBad && clientData.badMaterials.Length > 0)
        {
            chosenBodyMaterial =
                clientData.badMaterials[
                    Random.Range(0, clientData.badMaterials.Length)
                ];

            chosenFaceMaterial = clientData.badFaceMaterial;
        }
        else if (!isBad && clientData.goodMaterials.Length > 0)
        {
            chosenBodyMaterial =
                clientData.goodMaterials[
                    Random.Range(0, clientData.goodMaterials.Length)
                ];

            chosenFaceMaterial = clientData.goodFaceMaterial;
        }

        // Assign both materials
        if (chosenBodyMaterial != null &&
            chosenFaceMaterial != null)
        {
            Material[] mats = renderer.materials;

            // Safety check: model should have 2 slots
            if (mats.Length >= 2)
            {
                mats[0] = chosenBodyMaterial; // body
                mats[1] = chosenFaceMaterial; // face

                renderer.materials = mats;
            }
            else
            {
                Debug.LogWarning(
                    $"{obj.name} does not have 2 material slots!"
                );
            }
        }
    }

    ClientController controller =
        obj.GetComponent<ClientController>();
    
    // Assign random voice actor
    if (clientData.availableVoiceActors.Length > 0)
    {
        FaceController faceController =
            obj.GetComponentInChildren<FaceController>();

        if (faceController != null &&
            clientData.availableVoiceActors.Length > 0)
        {
            int randomVA =
                clientData.availableVoiceActors[
                    Random.Range(
                        0,
                        clientData.availableVoiceActors.Length
                    )
                ];

            faceController.SetVoiceActor(randomVA);
        }
    }
    else
    {
        Debug.LogWarning(
            $"{clientData.name} has no voice actors assigned!"
        );
    }

    controller.SetClient(
        clientData,
        randomName,
        isBad
    );

    queuingDevice.AddToOrderQueue(controller);

    currentClients++;
    clientsCount++;
    }
}
