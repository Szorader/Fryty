using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;

    public Transform spawnPointA;
    public Transform spawnPointB;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnAt(spawnPointA);
            SpawnAt(spawnPointB);

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnAt(Transform point)
    {
        if (carPrefabs.Length == 0)
        {
            Debug.Log("Brak prefabów");
            return;
        }

        GameObject car = carPrefabs[Random.Range(0, carPrefabs.Length)];
        GameObject spawned = Instantiate(car, point.position, point.rotation);

        //Debug.Log("Spawn: " + spawned.name + " at " + point.name);
    }
}