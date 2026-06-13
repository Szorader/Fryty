using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Collections;
public class ClickToSpawn : MonoBehaviour
{
    [Header("Prefab do zespawnowania")]
    public GameObject prefabToSpawn;

    [Header("Spawnu")] 
    [SerializeField] private Vector3 spawnPositionOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] private Vector3 launchDirection = Vector3.right;
    //public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);

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
    
    [Header("Animation")]
    [SerializeField] private Animator animator;

    private static List<GameObject> spawnedObjects = new List<GameObject>();

    void OnMouseDown()
    {
        StartCoroutine(SpawnObjectCoroutine());
    }

    private IEnumerator SpawnObjectCoroutine()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab nie jest przypisany!");
            yield break;
        }

        if (wallet == null)
        {
            Debug.LogWarning("Wallet nie jest przypisany!");
            yield break;
        }

        spawnedObjects.RemoveAll(item => item == null);

        if (spawnedObjects.Count >= maxObjects)
        {
            wallet.ShowError("You can't have more potatoes");
            yield break;
        }

        if (!wallet.HasMoney(cost))
        {
            wallet.ShowError("You don't have enough money");
            yield break;
        }

        wallet.SpendMoney(cost);

        if (animator != null)
        {
            animator.SetTrigger("open");
        }

        yield return new WaitForSeconds(0.2f);

        Vector3 spawnPosition = transform.position + spawnPositionOffset;

        RuntimeManager.PlayOneShot(popSound, spawnPosition);

        GameObject spawned = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawned);

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(launchDirection.normalized * force);
        }
    }
}