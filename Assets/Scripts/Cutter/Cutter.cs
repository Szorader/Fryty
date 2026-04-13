using UnityEngine;
using System.Collections;

public class Cutter : MonoBehaviour
{
    [Header("INPUT")]
    public string requiredTag = "Potato"; // tylko ziemniaki

    [Header("OUTPUT")]
    public GameObject friesPrefab;

    [Header("OUTPUT POINT")]
    public Transform outputPoint; // puste GameObject, z którego wychodzą frytki

    [Header("SPAWN SETTINGS")]
    public float spawnDelay = 2f;
    public float force = 5f;

    [Header("STATE")]
    public bool hasPotatoLoaded = false;
    public bool isProcessing = false;

    private bool canAcceptPotato => !hasPotatoLoaded && !isProcessing;

    // Trigger wlotu ziemniaka
    public void HandleTrigger(Collider other)
    {
        if (!canAcceptPotato) return;
        if (!other.CompareTag(requiredTag)) return;

        Destroy(other.gameObject);
        hasPotatoLoaded = true;

        Debug.Log("Potato loaded. Waiting for button press.");
    }

    // Wywołane przez przycisk
    public void PressButton(FriesData.FriesType selectedType)
    {
        if (!hasPotatoLoaded)
        {
            Debug.Log("No potato loaded.");
            return;
        }

        if (isProcessing) return;

        StartCoroutine(ProcessAndSpawn(selectedType));
    }

    private IEnumerator ProcessAndSpawn(FriesData.FriesType selectedType)
    {
        isProcessing = true;

        Debug.Log("Processing fries type: " + selectedType);

        yield return new WaitForSeconds(spawnDelay);

        // spawn w pozycji outputPoint lub fallback na transform + offset
        Vector3 spawnPosition = outputPoint != null ? outputPoint.position : transform.position;
        GameObject spawned = Instantiate(friesPrefab, spawnPosition, Quaternion.identity);

        // ustaw typ frytki
        FriesData friesData = spawned.GetComponent<FriesData>();
        if (friesData != null)
        {
            friesData.SetFriesType(selectedType);
        }

        // nadaj siłę do przodu
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = outputPoint != null ? outputPoint.forward : transform.forward;
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }

        hasPotatoLoaded = false;
        isProcessing = false;

        Debug.Log("Machine ready for next potato.");
    }
}