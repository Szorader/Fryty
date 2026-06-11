using UnityEngine;

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

    [Range(0f, 1f)]
    public float goodFriesBonus;

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

        float percentChange = 0f;

        // TIME
        int steps = Mathf.Max(0, (timeAlive - 30) / 10);
        percentChange += steps * penaltyPer10Seconds;

        // FRIES
        if (basket.friesType != order.fries)
            percentChange += wrongFriesPenalty;
        else
            percentChange += goodFriesBonus;

        // COOK
        if (basket.cookLevel == 1)
            percentChange += goodCookBonus;
        else
            percentChange += badCookPenalty;

        // SAUCE
        if (basket.sauceType != order.sauce)
            percentChange += wrongSaucePenalty;
        else
            percentChange += goodSauceBonus;

        // SEASONING
        if (basket.seasoningType != order.seasoning)
            percentChange += wrongSeasoningPenalty;
        else
            percentChange += goodSeasoningBonus;

        // PERFECT BONUS
        if (IsPerfectOrder(timeAlive, basket, order) && timeAlive <= 30)
            percentChange -= fastOrderBonus;

        // 🔥 KLUCZ: soft cap żeby system nie eksplodował
        percentChange = Mathf.Clamp(percentChange, -0.6f, 0.6f);

        float tip = baseTip + (baseTip * percentChange);

        tip = Mathf.Max(0f, tip);
        tip = Mathf.Round(tip * 100f) / 100f;

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

        return basket.friesType == order.fries &&
               basket.cookLevel == 1 &&
               basket.sauceType == order.sauce &&
               basket.seasoningType == order.seasoning;
    }
}