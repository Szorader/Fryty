using UnityEngine;
using System.Collections;

public class Fryer2 : MonoBehaviour
{
    [Header("Materials")]
    public Material emptyMaterial;
    public Material fullMaterial;
    public Material[] fryMaterials;

    [Header("XYZ")]
    public Vector3 localFryerPosition = new Vector3(0f, 0f, 0f);
    public Vector3 localLiftedPosition = new Vector3(0f, 2f, 0f);

    [Header("Czas etapów smażenia")]
    public float stepDelay = 2f;

    [Header("Fryta")]
    public GameObject fryPrefab;
    public float fryLaunchForce = 5f;

    [HideInInspector] public bool isFrying = false;

    private Renderer rend;
    private Coroutine fryingCoroutine;
    private int fryLevel = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = emptyMaterial;
    }
    
    void OnMouseDown()
    {
        if (isFrying)
        {
            StopFrying();
        }
    }
    
    public void StartFrying()
    {
        if (isFrying) return;

        isFrying = true;

        transform.localPosition = localFryerPosition;
        rend.material = fullMaterial;

        fryLevel = 0;

        fryingCoroutine = StartCoroutine(FryingProcess());
    }

    public void StopFrying()
    {
        if (!isFrying) return;

        if (fryingCoroutine != null)
            StopCoroutine(fryingCoroutine);

        isFrying = false;

        transform.localPosition = localLiftedPosition;

        LaunchFry(fryLevel - 1);

        rend.material = emptyMaterial;
    }

    IEnumerator FryingProcess()
    {
        while (fryLevel < fryMaterials.Length)
        {
            yield return new WaitForSeconds(stepDelay);
            rend.material = fryMaterials[fryLevel];
            fryLevel++;
        }
    }

    void LaunchFry(int materialIndex)
    {
        if (fryPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject fry = Instantiate(fryPrefab, spawnPos, Quaternion.identity);

        Renderer fryRend = fry.GetComponentInChildren<Renderer>();
        if (fryRend != null && fryMaterials.Length > 0)
        {
            materialIndex = Mathf.Clamp(materialIndex, 0, fryMaterials.Length - 1);
            fryRend.material = fryMaterials[materialIndex];
        }

        Rigidbody rb = fry.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * fryLaunchForce, ForceMode.Impulse);
        }
    }
}