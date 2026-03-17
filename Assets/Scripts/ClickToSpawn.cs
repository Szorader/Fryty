using UnityEngine;

public class ClickToSpawn : MonoBehaviour
{
    [Header("Prefab do zespawnowania")]
    public GameObject prefabToSpawn;

    [Header("Offset spawnu")]
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);

    [Header("Siła wystrzału")]
    public float force = 500f;

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

        Vector3 spawnPosition = transform.position + spawnOffset;

        GameObject spawned = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // 🔥 NOWA CZĘŚĆ – dodanie siły
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = spawnOffset.normalized; // kierunek "od obiektu"
            rb.AddForce(direction * force);
        }
        else
        {
            Debug.LogWarning("Spawned obiekt nie ma Rigidbody!");
        }
    }
}