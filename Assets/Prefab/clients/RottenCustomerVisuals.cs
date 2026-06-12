using UnityEngine;
using System.Collections.Generic;
public class RottenCustomerVisuals : MonoBehaviour
{
    [Header("Growths / Roots")]
    [SerializeField] private GameObject[] growths;
    [SerializeField] private GameObject horn;
    private const float hornChance = 0.1f; 

    [Header("Effects")]
    [SerializeField] private GameObject flyParticles;

    public void SetupBadVisuals()
    {
        // disable everything first
        foreach (GameObject growth in growths)
        {
            if (growth != null)
                growth.SetActive(false);
        }
        
        if (horn != null)
            horn.SetActive(false);

        if (flyParticles != null)
            flyParticles.SetActive(true);

        // pick 3 unique growths
        if (horn != null && Random.value < hornChance)
        {
            horn.SetActive(true);
            return;
        }
        
        List<int> availableIndices = new List<int>();

        for (int i = 0; i < growths.Length; i++)
            availableIndices.Add(i);

        int count = Mathf.Min(3, growths.Length);

        for (int i = 0; i < count; i++)
        {
            int randomIndex =
                Random.Range(0, availableIndices.Count);

            int growthIndex =
                availableIndices[randomIndex];

            availableIndices.RemoveAt(randomIndex);

            if (growths[growthIndex] != null)
                growths[growthIndex].SetActive(true);
        }
    }

    public void SetupGoodVisuals()
    {
        foreach (GameObject growth in growths)
        {
            if (growth != null)
                growth.SetActive(false);
        }

        if (flyParticles != null)
            flyParticles.SetActive(false);
        
        if (horn != null)
            horn.SetActive(false);
    }
}
