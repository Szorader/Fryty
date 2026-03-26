using UnityEngine;

public class ServingBasket : MonoBehaviour
{
    [Header("Fries")]
    [SerializeField] private string FryTag = "FriedFry";
    [SerializeField] private GameObject Fry;

    [Header("Sauce")]
    [SerializeField] private string SouceTag = "Souce";
    [SerializeField] private GameObject Souce;

    [Header("Opcjonalnie")]
    [SerializeField] private bool unlockOnlyOnce = true;

    private bool hasUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (unlockOnlyOnce && hasUnlocked)
            return;

        if (other.CompareTag(FryTag))
        {
            UnlockObject(Fry);
        }
        else if (other.CompareTag(SouceTag))
        {
            UnlockObject(Souce);
        }
    }

    private void UnlockObject(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Nie przypisano obiektu do odblokowania!");
            return;
        }

        targetObject.SetActive(true);
        hasUnlocked = true;
    }
}