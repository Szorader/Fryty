using UnityEngine;

/// <summary>
///  BASE TIP = 10
///  
///          GOOD       |	BAD
///  TYPE	    +5%	    |	-30%
///  COOK	    +5%	    |	-50%
///  SAUCE  	+10%	|	-30%
///  SEAS.  	+5%	    |	-20%
///  TIME       +5%     |   -5%
///  
///  Wszystko liczone od aktualnej wartości napiwku
/// </summary>
public class CustomerSatisfaction : MonoBehaviour
{
    [Header("BASE TIP")]
    public float baseTip = 2f;

    [Header("TIME PENALTY")] 
    [Range(0f, 1f)]
    public float penaltyPer10Seconds;

    [Range(0f, 1f)]

    public float fastOrderBonus;

    [Header("FRIES")]
    [Range(0f, 1f)]
    public float wrongFriesPenalty;

    [Range(0f, 1f)] public float goodFriesBonus;

    [Header("COOK LEVEL")]
    [Range(0f, 1f)]
    public float badCookPenalty;

    [Range(0f, 1f)]
    public float goodCookBonus;

    [Header("SAUCE")]
    [Range(0f, 1f)]
    public float wrongSaucePenalty;

    [Range(0f, 1f)]
    public float goodSauceBonus;

    [Header("SEASONING")]
    [Range(0f, 1f)]
    public float wrongSeasoningPenalty;

    [Range(0f, 1f)]
    public float goodSeasoningBonus;

    public float CalculateTip(
        int timeAlive,
        BasketData basket,
        CustomerOrder order
    )
    {
        if (!basket || !order)
            return 0f;

        float tip = baseTip;

        // TIME PENALTY (always applies)
        int adjustedTime = Mathf.Max(0, timeAlive - 30);

        for (int i = 0; i < adjustedTime / 10; i++)
        {
            tip -= tip * penaltyPer10Seconds;
        }

        // FRIES TYPE
        if (basket.friesType != order.fries)
            tip -= tip * wrongFriesPenalty;
        else
            tip += tip * goodFriesBonus;

        // COOK LEVEL
        if (basket.cookLevel == 1)
            tip += tip * goodCookBonus;
        else
            tip -= tip * badCookPenalty;

        // SAUCE
        if (basket.sauceType != order.sauce)
            tip -= tip * wrongSaucePenalty;
        else
            tip += tip * goodSauceBonus;

        // SEASONING
        if (basket.seasoningType != order.seasoning)
            tip -= tip * wrongSeasoningPenalty;
        else
            tip += tip * goodSeasoningBonus;

        // PERFECT CHECK
        bool perfect = IsPerfectOrder(timeAlive, basket, order);
 
        // TIME BONUS ONLY IF PERFECT
        if (perfect && timeAlive <= 30)
        {
            tip += tip * fastOrderBonus;
        }

        tip = Mathf.Max(0f, tip);
        tip = Mathf.Round(tip * 100f) / 100f;
        if (tip > 4f)
            return 4f;
        return tip;
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