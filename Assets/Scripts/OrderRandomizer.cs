using UnityEngine;

public class OrderRandomizer : MonoBehaviour
{
    public OrderDatabase database;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRandomOrder();
        }
    }

    private void GenerateRandomOrder()
    {
        // losowanie frytek
        FriesData.FriesType fries = database.availableFries[Random.Range(0, database.availableFries.Length)];

        // losowanie sosu
        string sauce = database.availableSauces[Random.Range(0, database.availableSauces.Length)];

        // losowanie przyprawy
        string seasoning = database.availableSeasonings[Random.Range(0, database.availableSeasonings.Length)];

        Debug.Log($"Order: {fries}, {sauce}, {seasoning}");
    }
}