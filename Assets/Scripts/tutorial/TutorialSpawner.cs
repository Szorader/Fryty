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
    private const int maxTutorialClients = 2;

    void Start()
    {
        queuingDevice = FindObjectOfType<QueuingDevice>();

        // spawn first GOOD customer immediately
        SpawnClient(false);

        // start waiting for second spawn
        StartCoroutine(TutorialFlow());
    }

    private IEnumerator TutorialFlow()
    {
        // wait until tutorial allows next spawn
        yield return new WaitUntil(() => canSpawn);

        // wait until first customer fully leaves queue
        yield return new WaitUntil(() =>
            queuingDevice.orderQueue.Count == 0
        );

        // spawn EVIL customer
        SpawnClient(true);
    }

    private void SpawnClient(bool isBad)
    {
        if (spawnedClients >= maxTutorialClients)
        {
            Debug.Log("Tutorial clients finished");
            return;
        }

        ClientData clientData =
            clients[Random.Range(0, clients.Length)];

        string randomName =
            clientNames[
                Random.Range(0, clientNames.Length)
            ];

        GameObject obj = Instantiate(
            clientData.clientPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // growths / flies
        RottenCustomerVisuals visuals =
            obj.GetComponent<RottenCustomerVisuals>();

        if (visuals != null)
        {
            if (isBad)
                visuals.SetupBadVisuals();
            else
                visuals.SetupGoodVisuals();
        }
        
        Renderer renderer =
            obj.GetComponentInChildren<SkinnedMeshRenderer>();

        if (renderer != null)
        {
            Material chosenBodyMaterial = null;
            Material chosenFaceMaterial = null;

            // GOOD customer
            if (!isBad)
            {
                if (clientData.goodMaterials.Length > 0)
                {
                    chosenBodyMaterial =
                        clientData.goodMaterials[
                            Random.Range(
                                0,
                                clientData.goodMaterials.Length
                            )
                        ];
                }

                chosenFaceMaterial =
                    clientData.goodFaceMaterial;
            }
            // BAD customer
            else
            {
                if (clientData.badMaterials.Length > 0)
                {
                    chosenBodyMaterial =
                        clientData.badMaterials[
                            Random.Range(
                                0,
                                clientData.badMaterials.Length
                            )
                        ];
                }

                chosenFaceMaterial =
                    clientData.badFaceMaterial;
            }

            // assign body + face material
            Material[] mats = renderer.materials;

            if (mats.Length >= 2)
            {
                mats[0] = chosenBodyMaterial;
                mats[1] = chosenFaceMaterial;

                renderer.materials = mats;
            }
        }

        // setup client
        ClientController controller =
            obj.GetComponent<ClientController>();

        controller.SetClient(
            clientData,
            randomName,
            isBad
        );

        // RANDOM VA
        FaceController face =
            obj.GetComponentInChildren<FaceController>();

        if (face != null &&
            clientData.availableVoiceActors.Length > 0)
        {
            int randomVA =
                clientData.availableVoiceActors[
                    Random.Range(
                        0,
                        clientData.availableVoiceActors.Length
                    )
                ];

            face.SetVoiceActor(randomVA);
        }

        // add to queue
        queuingDevice.AddToOrderQueue(
            controller
        );

        spawnedClients++;

        Debug.Log(
            $"Spawned tutorial client | " +
            $"Bad: {isBad}"
        );
    }
}