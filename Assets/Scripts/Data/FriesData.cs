using UnityEngine;

public class FriesData : MonoBehaviour
{
    public enum FriesType
    {
        Straight,
        Crinkle,
        Wedges
    }

    [Header("TYPE")]
    public FriesType friesType;

    [Header("COOK LEVEL")]
    [Range(0, 4)]
    public int cookLevel = 0;
    public string cookDes;
    
    [Header("FRIES MODELS")]
    public GameObject straightModel;
    public GameObject crinkleModel;
    public GameObject wedgesModel;
    
    [Header("COOK MATERIALS")]
    public Material[] cookMaterials = new Material[5];

    private void Start()
    {
        RefreshVisuals();
    }

    private void OnValidate()
    {
        RefreshVisuals();
    }

    public void SetFriesType(FriesType newType)
    {
        friesType = newType;
        RefreshVisuals();
    }

    public void RefreshVisuals()
    {
        UpdateCookDescription();
        UpdateFriesModel();
        UpdateCookMaterial();
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

    private void UpdateFriesModel()
    {
        straightModel.SetActive(false);
        crinkleModel.SetActive(false);
        wedgesModel.SetActive(false);

        switch (friesType)
        {
            case FriesType.Straight: straightModel.SetActive(true); break;
            case FriesType.Crinkle: crinkleModel.SetActive(true); break;
            case FriesType.Wedges: wedgesModel.SetActive(true); break;
        }
    }

    private void UpdateCookMaterial()
    {
        Material mat = cookMaterials[cookLevel];

        ApplyMaterialToModel(straightModel, mat);
        ApplyMaterialToModel(crinkleModel, mat);
        ApplyMaterialToModel(wedgesModel, mat);
    }

    private void ApplyMaterialToModel(GameObject targetModel, Material material)
    {
        Renderer[] renderers = targetModel.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
            r.sharedMaterial = material;
    }
}