using UnityEngine;

public class BasketInteraction : MonoBehaviour
{
    public BasketData basketData;
    public GameObject ketchupBottle;
    public GameObject mayoBottle;
    public GameObject cheeseBottle;
    public GameObject emptySauceBox;
    public GameObject saltShaker;
    public GameObject pepperShaker;
    public GameObject bell;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleClick(hit.collider.gameObject);
            }
        }
    }

    private void HandleClick(GameObject clicked)
    {
        if (clicked == ketchupBottle) TrySetSauce(BasketData.SauceType.Ketchup);
        else if (clicked == mayoBottle) TrySetSauce(BasketData.SauceType.Mayo);
        else if (clicked == cheeseBottle) TrySetSauce(BasketData.SauceType.Cheese);
        else if (clicked == emptySauceBox) TrySetSauce(BasketData.SauceType.None, true);
        else if (clicked == saltShaker) TrySetSeasoning(BasketData.SeasoningType.Salt);
        else if (clicked == pepperShaker) TrySetSeasoning(BasketData.SeasoningType.Pepper);
        else if (clicked == bell) ResetBasket();
    }

    private void TrySetSauce(BasketData.SauceType newSauce, bool force = false)
    {
        if (basketData.sauceType != BasketData.SauceType.None && !force) return;
        basketData.sauceType = newSauce;
        basketData.RefreshVisuals();
    }

    private void TrySetSeasoning(BasketData.SeasoningType newSeasoning)
    {
        if (basketData.seasoningType != BasketData.SeasoningType.None) return;
        basketData.seasoningType = newSeasoning;
        basketData.RefreshVisuals();
    }

    private void ResetBasket()
    {
        basketData.friesType = BasketData.FriesType.None;
        basketData.cookLevel = 0;
        basketData.sauceType = BasketData.SauceType.None;
        basketData.seasoningType = BasketData.SeasoningType.None;
        basketData.RefreshVisuals();
    }
}