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

        //COOK LEVEL
        if (basket.cookLevel == 0 || basket.cookLevel == 4)
        {
            tip -= burnedOrRawPenalty;
        }
        else if (basket.cookLevel != 2)
        {
            tip -= badCookPenalty;
        }
        else
        {
            tip += perfectBonus;
        }

        //SAUCE
        if (basket.sauceType != order.sauce)
            tip -= wrongSaucePenalty;

        //SEASONING
        if (basket.seasoningType != order.seasoning)
            tip -= wrongSeasoningPenalty;

        return Mathf.Max(0f, tip);
    }
}