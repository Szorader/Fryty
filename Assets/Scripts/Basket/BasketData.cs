using UnityEngine;

[ExecuteAlways]
public class BasketData : MonoBehaviour
{
    public enum FriesType { Straight, Crinkle, Wedges, None }
    [Header("FRIES")]
    public FriesType friesType = FriesType.None;
    [Range(0, 4)]
    public int cookLevel = 0;
    public string cookDes;

    public enum SauceType { None, Ketchup, Mayo, Cheese }
    [Header("SAUCE")]
    public SauceType sauceType = SauceType.None;

    public enum SeasoningType { None, Salt, Pepper }
    [Header("SEASONING")]
    public SeasoningType seasoningType = SeasoningType.None;

    [Header("FRIES MODELS")]
    public GameObject straightModel;
    public GameObject crinkleModel;
    public GameObject wedgesModel;

    [Header("COOK MATERIALS")]
    public Material[] cookMaterials = new Material[5];

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
        switch (cookLevel)
        {
            case 0: cookDes = "Raw"; break;
            case 1: cookDes = "Lightly Cooked"; break;
            case 2: cookDes = "Perfect"; break;
            case 3: cookDes = "Overcooked"; break;
            case 4: cookDes = "Burnt"; break;
            default: cookDes = "Unknown"; break;
        }
    }

    private void UpdateFriesVisual()
    {
        straightModel.SetActive(false);
        crinkleModel.SetActive(false);
        wedgesModel.SetActive(false);

        if (friesType == FriesType.None) return;

        GameObject activeFries = friesType == FriesType.Straight ? straightModel :
                                 friesType == FriesType.Crinkle ? crinkleModel :
                                 wedgesModel;

        activeFries.SetActive(true);

        Material mat = cookMaterials[cookLevel];
        Renderer[] renderers = activeFries.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers) r.sharedMaterial = mat;
    }

    private void UpdateSauceVisual()
    {
        emptySauceModel.SetActive(false);
        ketchupModel.SetActive(false);
        mayoModel.SetActive(false);
        cheeseModel.SetActive(false);

        GameObject activeSauce = sauceType == SauceType.None ? emptySauceModel :
                                 sauceType == SauceType.Ketchup ? ketchupModel :
                                 sauceType == SauceType.Mayo ? mayoModel :
                                 cheeseModel;

        activeSauce.SetActive(true);
    }

    private void UpdateSeasoningVisual()
    {
        saltModel.SetActive(false);
        pepperModel.SetActive(false);

        if (seasoningType == SeasoningType.None) return;

        GameObject activeSeasoning = seasoningType == SeasoningType.Salt ? saltModel : pepperModel;
        activeSeasoning.SetActive(true);
    }

    private void ApplyMaterialToModel(GameObject targetModel, Material material)
    {
        Renderer[] renderers = targetModel.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers) r.sharedMaterial = material;
    }
}