using UnityEngine;

public class FriesData : MonoBehaviour
{
    [Header("TYPE")]
    public OrderDatabase.FriesType friesType;

    [Header("COOK LEVEL")]
    [Range(0, 4)]
    public int cookLevel = 0;
    public string cookDes;
    public bool isFried = false;

    [Header("FRIES MODELS")]
    public GameObject straightModel;
    public GameObject crinkleModel;
    public GameObject wedgesModel;

    [Header("COOK MATERIALS")]
    public Material[] cookMaterials;

    private void Start()
    {
        RefreshVisuals();
    }

    private void OnValidate()
    {
        RefreshVisuals();
    }

    public void SetFriesType(OrderDatabase.FriesType newType)
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

    private void UpdateFriesModel()
    {
        if (straightModel) straightModel.SetActive(false);
        if (crinkleModel) crinkleModel.SetActive(false);
        if (wedgesModel) wedgesModel.SetActive(false);

        switch (friesType)
        {
            case OrderDatabase.FriesType.Straight:
                if (straightModel) straightModel.SetActive(true);
                break;

            case OrderDatabase.FriesType.Crinkle:
                if (crinkleModel) crinkleModel.SetActive(true);
                break;

            case OrderDatabase.FriesType.Wedges:
                if (wedgesModel) wedgesModel.SetActive(true);
                break;
        }
    }

    private void UpdateCookMaterial()
    {
        if (cookMaterials == null || cookMaterials.Length == 0) return;

        int index = Mathf.Clamp(cookLevel, 0, cookMaterials.Length - 1);
        Material mat = cookMaterials[index];

        ApplyMaterial(straightModel, mat);
        ApplyMaterial(crinkleModel, mat);
        ApplyMaterial(wedgesModel, mat);
    }

    private void ApplyMaterial(GameObject model, Material material)
    {
        if (!model || !material) return;

        var renderers = model.GetComponentsInChildren<Renderer>(true);
        foreach (var r in renderers)
            r.sharedMaterial = material;
    }
}