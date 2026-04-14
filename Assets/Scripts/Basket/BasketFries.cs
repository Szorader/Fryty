using UnityEngine;
using System.Collections;

public class BasketFries : MonoBehaviour
{
    public BasketData basketData;

    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessing) return;
        if (basketData == null) return;
        if (!other.CompareTag("Fry")) return;
        if (basketData.friesType != OrderDatabase.FriesType.None) return;

        FriesData fries = other.GetComponent<FriesData>();
        if (fries == null)
            fries = other.GetComponentInParent<FriesData>();

        if (fries == null) return;

        StartCoroutine(TransferFries(fries));
    }

    private IEnumerator TransferFries(FriesData fries)
    {
        isProcessing = true;

        yield return null;

        if (fries == null)
        {
            isProcessing = false;
            yield break;
        }

        basketData.friesType = fries.friesType;
        basketData.cookLevel = fries.cookLevel;

        basketData.RefreshVisuals();

        Destroy(fries.gameObject);

        isProcessing = false;
    }

    public void ResetFries()
    {
        if (basketData == null) return;

        basketData.friesType = OrderDatabase.FriesType.None;
        basketData.cookLevel = 0;

        basketData.RefreshVisuals();
    }
}