using UnityEngine;
using System.Collections.Generic;
public class RottenCustomerVisuals : MonoBehaviour
{
    [Header("Growths / Roots")]
    [SerializeField] private GameObject[] growths;

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

        if (flyParticles != null)
            flyParticles.SetActive(true);

        // pick 3 unique growths
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
    }
}
