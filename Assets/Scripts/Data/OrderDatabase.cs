using UnityEngine;

[CreateAssetMenu(fileName = "OrderDatabase", menuName = "Food/Order Database")]
public class OrderDatabase : ScriptableObject
{
    public enum FriesType
    {
        None,
        Straight,
        Crinkle,
        Wedges
    }
    
    public enum SauceType
    {
        None,
        Ketchup,
        Mayo,
        Cheese
    }

    public enum SeasoningType
    {
        None,
        Salt,
        Pepper
    }

    [Header("Fries Types")]
    public FriesType[] availableFries;

    [Header("Sauces")]
    public SauceType[] availableSauces;

    [Header("Seasonings")]
    public SeasoningType[] availableSeasonings;
}