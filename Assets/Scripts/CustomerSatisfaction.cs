using UnityEngine;
/// <summary>
///  BASE TIP = 10
///  
///          GUT    |	BAD
///  TYPE	    +1	|	-3
///  COOK	    +1	|	-6
///  SOUCE  	+2	|	-4
///  SEAS.  	+1	|	-2
///  
///  WORST TIP = -15 ~0
///  BEST TIP = +15
/// </summary>
public class CustomerSatisfaction : MonoBehaviour
{
    [Header("BASE TIP")]
    public float baseTip = 10f;

    [Header("TIME PENALTY")]
    public float penaltyPer10Seconds = 1f;

    [Header("FRIES")]
    public float wrongFriesPenalty = 3f;
    public float goodFriesBonus = 1f;

    public float badCookPenalty = 6f;
    public float goodCookBonus = 1f;

    [Header("SAUCE")]
    public float wrongSaucePenalty = 4f;
    public float goodSauceBonus = 2f;

    [Header("SEASONING")]
    public float wrongSeasoningPenalty = 2f;
    public float goodSeasoningBonus = 1f;

    public float CalculateTip(
        int timeAlive,
        BasketData basket,
        CustomerOrder order
    )
    {
        float tip = baseTip;

        if (!basket || !order)
            return 0f;

        // TIME
        int adjustedTime = Mathf.Max(0, timeAlive - 30);
        tip -= (adjustedTime / 10) * penaltyPer10Seconds;

        // FRIES TYPE
        if (basket.friesType != order.fries)
            tip -= wrongFriesPenalty;
        else
            tip += goodFriesBonus;

        // COOK LEVEL
        if (basket.cookLevel == 1)
            tip += goodCookBonus;
        else
            tip -= badCookPenalty;

        // SAUCE
        if (basket.sauceType != order.sauce)
            tip -= wrongSaucePenalty;
        else
            tip += goodSauceBonus;

        // SEASONING
        if (basket.seasoningType != order.seasoning)
            tip -= wrongSeasoningPenalty;
        else
            tip += goodSeasoningBonus;

        return Mathf.Max(0f, tip);
    }

    public bool IsPerfectOrder(
        int timeAlive,
        BasketData basket,
        CustomerOrder order
    )
    {
        if (!basket || !order)
            return false;

        if (basket.friesType != order.fries)
            return false;

        if (basket.cookLevel != 1)
            return false;

        if (basket.sauceType != order.sauce)
            return false;

        if (basket.seasoningType != order.seasoning)
            return false;

        return true;
    }
}