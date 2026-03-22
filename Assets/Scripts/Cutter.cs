using UnityEngine;
using System.Collections;

public class Cutter : MonoBehaviour
{
    public string requiredTag = "Item";
    public GameObject prefabToSpawn;
    public float spawnDelay = 2f;
    public Vector3 spawnOffset = new Vector3(0f, 1f, 2f);
    public float force = 500f;
    
    public void HandleTrigger(Collider other)
    {
        if (!other.CompareTag(requiredTag)) return;

        Destroy(other.gameObject);
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject spawned = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = spawnOffset.normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}