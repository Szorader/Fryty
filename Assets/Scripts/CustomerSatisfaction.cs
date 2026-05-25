using UnityEngine;

public class CustomerSatisfaction : MonoBehaviour
{
    [Header("BASE TIP")]
    public float baseTip = 10f;

    [Header("TIME PENALTY")]
    public float penaltyPer10Seconds = 0.5f;

    [Header("FRIES PENALTIES")]
    public float wrongFriesPenalty = 5f;
    public float badCookPenalty = 2f;
    public float burnedOrRawPenalty = 5f;
    public float perfectBonus = 1f;

    [Header("SAUCE PENALTY")]
    public float wrongSaucePenalty = 3f;

    [Header("SEASONING PENALTY")]
    public float wrongSeasoningPenalty = 1f;

    public float CalculateTip(
        int timeAlive,
        BasketData basket,
        CustomerOrder order
    )
    {
        float tip = baseTip;

        if (!basket || !order)
            return 0f;

        //TIME
        int adjustedTime = Mathf.Max(0, timeAlive - 30);
        tip -= (adjustedTime / 10) * penaltyPer10Seconds;

        //FRIES TYPE
        if (basket.friesType != order.fries)
            tip -= wrongFriesPenalty;

        // COOK LEVEL
        if (basket.cookLevel == 0 || basket.cookLevel == 2)
        {
            tip -= burnedOrRawPenalty; // teraz = 7
        }

        //SAUCE
        if (basket.sauceType != order.sauce)
            tip -= wrongSaucePenalty;

        //SEASONING
        if (basket.seasoningType != order.seasoning)
            tip -= wrongSeasoningPenalty;

        return Mathf.Max(0f, tip);
    }
    
    /// <summary>
    /// Used ONLY for customer reactions
    /// (happy / sad / angry).
    ///
    /// Time DOES NOT matter here.
    /// Only order correctness.
    /// </summary>
    public bool IsPerfectOrder(
        int timeAlive,
        BasketData basket,
        CustomerOrder order
    )
    {
        if (!basket || !order)
            return false;

        // Correct fries type
        if (basket.friesType != order.fries)
        {
            Debug.Log("Wrong fries");
            return false;
        }

        // Perfect cook only
        if (basket.cookLevel != 1)
        {
            Debug.Log("Wrong cook level");
            return false;
        }

        // Correct sauce
        if (basket.sauceType != order.sauce)
        {
            Debug.Log("Wrong sauce");
            return false;
        }

        // Correct seasoning
        if (basket.seasoningType != order.seasoning)
        {
            Debug.Log("Wrong seasoning");
            return false;
        }

        Debug.Log("PERFECT ORDER");
        return true;
    }
}