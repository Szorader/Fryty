using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public OrderDatabase database;

    [Header("ORDER")]
    public OrderDatabase.FriesType fries;
    public OrderDatabase.SauceType sauce;
    public OrderDatabase.SeasoningType seasoning;

    private void Start()
    {
        GenerateOrder();
    }

    public void GenerateOrder()
    {
        if (!database) return;

        fries = database.availableFries[Random.Range(0, database.availableFries.Length)];
        sauce = database.availableSauces[Random.Range(0, database.availableSauces.Length)];
        seasoning = database.availableSeasonings[Random.Range(0, database.availableSeasonings.Length)];
    }
}