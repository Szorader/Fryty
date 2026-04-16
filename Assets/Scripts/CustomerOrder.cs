using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour
{
    public OrderDatabase database;

    [Header("ORDER")]
    public OrderDatabase.FriesType fries;
    public OrderDatabase.SauceType sauce;
    public OrderDatabase.SeasoningType seasoning;
    
    public TMP_Text nameText;
    public TMP_Text orderText;
    
    public string clientName;

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
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        nameText.text = clientName;
        
        string friesText = fries.ToString();
        string sauceText = sauce.ToString();
        string seasoningText = seasoning.ToString();
        
        if (friesText == "None") friesText = "-";
        if (sauceText == "None") sauceText = "-";
        if (seasoningText == "None") seasoningText = "-";

        orderText.text =
            "Fries: " + friesText + "\n" +
            "Sauce: " + sauceText + "\n" +
            "Seasoning: " + seasoningText;
    }
        
    
}