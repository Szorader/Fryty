using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderTicketUI : MonoBehaviour
{
    [Header("UI")]
    public Image friesImage;
    public Image sauceImage;
    public Image seasoningImage;
    public TextMeshProUGUI orderText;

    [Header("Sprites - Fries")]
    public Sprite noneFries;
    public Sprite straightFries;
    public Sprite crinkleFries;
    public Sprite wedgesFries;

    [Header("Sprites - Sauces")]
    public Sprite noneSauce;
    public Sprite ketchup;
    public Sprite mayo;
    public Sprite cheese;
    public Sprite chili;
    public Sprite oneIsland;
    public Sprite garlic;

    [Header("Sprites - Seasoning")]
    public Sprite noneSeasoning;
    public Sprite salt;
    public Sprite pepper;

    public void SetOrder(string clientName,
        int orderNumber,
        OrderDatabase.FriesType fries,
        OrderDatabase.SauceType sauce,
        OrderDatabase.SeasoningType seasoning)
    {
        orderText.text = clientName;

        friesImage.sprite = GetFriesSprite(fries);
        sauceImage.sprite = GetSauceSprite(sauce);
        seasoningImage.sprite = GetSeasoningSprite(seasoning);
    }

    Sprite GetFriesSprite(OrderDatabase.FriesType type)
    {
        switch (type)
        {
            case OrderDatabase.FriesType.Straight: return straightFries;
            case OrderDatabase.FriesType.Crinkle: return crinkleFries;
            case OrderDatabase.FriesType.Wedges: return wedgesFries;
            default: return noneFries;
        }
    }

    Sprite GetSauceSprite(OrderDatabase.SauceType type)
    {
        switch (type)
        {
            case OrderDatabase.SauceType.Ketchup: return ketchup;
            case OrderDatabase.SauceType.Mayo: return mayo;
            case OrderDatabase.SauceType.Cheese: return cheese;
            case OrderDatabase.SauceType.Chili: return chili;
            case OrderDatabase.SauceType.OneIsland: return oneIsland;
            case OrderDatabase.SauceType.Garlic: return garlic;
            default: return noneSauce;
        }
    }

    Sprite GetSeasoningSprite(OrderDatabase.SeasoningType type)
    {
        switch (type)
        {
            case OrderDatabase.SeasoningType.Salt: return salt;
            case OrderDatabase.SeasoningType.Pepper: return pepper;
            default: return noneSeasoning;
        }
    }
}