using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class BasketInteraction : MonoBehaviour
{
    public BasketData basketData;

    [Header("REFERENCES")]
    public CustomerOrder currentCustomer;
    public CustomerWaitingTime waitingTime;
    public CustomerSatisfaction satisfaction;
    public bool isBad;

    public GameObject ketchupBottle;
    public GameObject mayoBottle;
    public GameObject cheeseBottle;
    public GameObject emptySauceBox;

    public GameObject saltShaker;
    public GameObject pepperShaker;

    public GameObject bell;
    
    public QueueManager queueManager;

    public float money = -5f;
    public TMP_Text moneyText;


    void Start()
    {
        UpdateMoney(0f);
    }
    private void Update()
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
        if (!basketData) return;

        if (clicked == ketchupBottle) TrySetSauce(OrderDatabase.SauceType.Ketchup);
        else if (clicked == mayoBottle) TrySetSauce(OrderDatabase.SauceType.Mayo);
        else if (clicked == cheeseBottle) TrySetSauce(OrderDatabase.SauceType.Cheese);
        else if (clicked == emptySauceBox) TrySetSauce(OrderDatabase.SauceType.None, true);

        else if (clicked == saltShaker) TrySetSeasoning(OrderDatabase.SeasoningType.Salt);
        else if (clicked == pepperShaker) TrySetSeasoning(OrderDatabase.SeasoningType.Pepper);

        else if (clicked == bell)
        {
            CheckOrder();
            ResetBasket();
        }
    }

    private void TrySetSauce(OrderDatabase.SauceType newSauce, bool force = false)
    {
        if (basketData.sauceType != OrderDatabase.SauceType.None && !force) return;

        basketData.sauceType = newSauce;
        basketData.RefreshVisuals();
    }

    private void TrySetSeasoning(OrderDatabase.SeasoningType newSeasoning)
    {
        if (basketData.seasoningType != OrderDatabase.SeasoningType.None) return;

        basketData.seasoningType = newSeasoning;
        basketData.RefreshVisuals();
    }

    private void CheckOrder()
    {
        if (!currentCustomer || !satisfaction || !basketData) return;

        float tip = satisfaction.CalculateTip(
            waitingTime.GetTime(),
            basketData,
            currentCustomer
        );

        Debug.Log("TIP: " + tip);
        UpdateMoney(tip);
        
        queueManager.ServeNextClient();
    }

    public void UpdateMoney(float amount)
    {
        if (isBad)
        {
            money -= +  15 + amount;
        }
        else
        {
            money += +  5 + amount;
        }
        
        moneyText.text = money.ToString();
    }
    
    public void UpdateMoneyKill(float amount)
    {
        money += amount;
        
        moneyText.text = money.ToString();
    }
    private void ResetBasket()
    {
        basketData.friesType = OrderDatabase.FriesType.None;
        basketData.cookLevel = 0;
        basketData.sauceType = OrderDatabase.SauceType.None;
        basketData.seasoningType = OrderDatabase.SeasoningType.None;

        basketData.RefreshVisuals();
    }
}