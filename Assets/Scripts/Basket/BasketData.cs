using UnityEngine;

[ExecuteAlways]
public class BasketData : MonoBehaviour
{
    [Header("FRIES")]
    public OrderDatabase.FriesType friesType = OrderDatabase.FriesType.None;

    [Range(0, 4)]
    public int cookLevel = 0;
    public string cookDes;

    [Header("SAUCE")]
    public OrderDatabase.SauceType sauceType = OrderDatabase.SauceType.None;

    [Header("SEASONING")]
    public OrderDatabase.SeasoningType seasoningType = OrderDatabase.SeasoningType.None;

    [Header("FRIES MODELS")]
    public GameObject straightModel;
    public GameObject crinkleModel;
    public GameObject wedgesModel;

    [Header("COOK MATERIALS")]
    public Material[] cookMaterials;

    [Header("SAUCE MODELS")]
    public GameObject emptySauceModel;
    public GameObject ketchupModel;
    public GameObject mayoModel;
    public GameObject cheeseModel;

    [Header("SEASONING MODELS")]
    public GameObject saltModel;
    public GameObject pepperModel;

    private void OnValidate()
    {
        RefreshVisuals();
    }

    public void RefreshVisuals()
    {
        UpdateCookDescription();
        UpdateFriesVisual();
        UpdateSauceVisual();
        UpdateSeasoningVisual();
    }

    private void UpdateCookDescription()
    {
        cookDes = cookLevel switch
        {
            0 => "Raw",
            1 => "Lightly Cooked",
            2 => "Perfect",
            3 => "Overcooked",
            4 => "Burnt",
            _ => "Unknown"
        };
    }

    private void UpdateFriesVisual()
    {
        if (straightModel) straightModel.SetActive(false);
        if (crinkleModel) crinkleModel.SetActive(false);
        if (wedgesModel) wedgesModel.SetActive(false);

        if (friesType == OrderDatabase.FriesType.None) return;

        GameObject activeFries = friesType switch
        {
            OrderDatabase.FriesType.Straight => straightModel,
            OrderDatabase.FriesType.Crinkle => crinkleModel,
            OrderDatabase.FriesType.Wedges => wedgesModel,
            _ => null
        };

        if (!activeFries) return;

        activeFries.SetActive(true);

        if (cookMaterials != null && cookMaterials.Length > 0)
        {
            int index = Mathf.Clamp(cookLevel, 0, cookMaterials.Length - 1);
            Material mat = cookMaterials[index];

            Renderer[] renderers = activeFries.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
                r.sharedMaterial = mat;
        }
    }

    private void UpdateSauceVisual()
    {
        if (emptySauceModel) emptySauceModel.SetActive(false);
        if (ketchupModel) ketchupModel.SetActive(false);
        if (mayoModel) mayoModel.SetActive(false);
        if (cheeseModel) cheeseModel.SetActive(false);

        GameObject activeSauce = sauceType switch
        {
            OrderDatabase.SauceType.None => emptySauceModel,
            OrderDatabase.SauceType.Ketchup => ketchupModel,
            OrderDatabase.SauceType.Mayo => mayoModel,
            OrderDatabase.SauceType.Cheese => cheeseModel,
            _ => null
        };

        if (activeSauce) activeSauce.SetActive(true);
    }

    private void UpdateSeasoningVisual()
    {
        if (saltModel) saltModel.SetActive(false);
        if (pepperModel) pepperModel.SetActive(false);

        if (seasoningType == OrderDatabase.SeasoningType.None) return;

        GameObject active = seasoningType == OrderDatabase.SeasoningType.Salt
            ? saltModel
            : pepperModel;

        if (active) active.SetActive(true);
    }
}