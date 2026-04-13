using UnityEngine;

public class BasketFries : MonoBehaviour
{
    public BasketData basketData;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fry")) return;
        if (basketData.friesType != BasketData.FriesType.None) return;

        FriesData fries = other.GetComponentInParent<FriesData>();

        basketData.friesType = (BasketData.FriesType)fries.friesType;
        basketData.cookLevel = fries.cookLevel;

        basketData.RefreshVisuals();

        Destroy(fries.gameObject);
    }

    public void ResetFries()
    {
        basketData.friesType = BasketData.FriesType.None;
        basketData.cookLevel = 0;
        basketData.RefreshVisuals();
    }
}