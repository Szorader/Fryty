using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ClickToSpawn : MonoBehaviour
{
    [Header("Prefab do zespawnowania")]
    public GameObject prefabToSpawn;

    [Header("Offset spawnu")]
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);

    [Header("Siła wystrzału")]
    public float force = 500f;

    [Header("Limit obiektów")]
    public int maxObjects = 5;

    [Header("Koszt")]
    [SerializeField] private float cost = 2f;

    [Header("Wallet")]
    [SerializeField] private Wallet wallet;

    [Header("Audio")]
    [SerializeField] private EventReference popSound;

    private static List<GameObject> spawnedObjects = new List<GameObject>();

    void OnMouseDown()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab nie jest przypisany!");
            return;
        }

        if (wallet == null)
        {
            Debug.LogWarning("Wallet nie jest przypisany!");
            return;
        }

        // cleanup nulli
        spawnedObjects.RemoveAll(item => item == null);

        // limit
        if (spawnedObjects.Count >= maxObjects)
        {
            Debug.Log("Limit obiektów osiągnięty (5).");
            return;
        }

        // TU: dopiero jeśli spawn jest możliwy
        wallet.SpendMoney(cost);

        Vector3 spawnPosition = transform.position + spawnOffset;

        RuntimeManager.PlayOneShot(popSound, spawnPosition);

        GameObject spawned = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawned);

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = spawnOffset.normalized;
            rb.AddForce(direction * force);
        }
        else
        {
            Debug.LogWarning("Spawned obiekt nie ma Rigidbody!");
        }
    }
}