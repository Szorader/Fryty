using UnityEngine;

[CreateAssetMenu(fileName = "OrderDatabase", menuName = "Food/Order Database")]
public class OrderDatabase : ScriptableObject
{
    [Header("Fries Types")]
    public FriesData.FriesType[] availableFries;

    [Header("Sauces")]
    public string[] availableSauces;

    [Header("Seasonings (including salt options)")]
    public string[] availableSeasonings;
}