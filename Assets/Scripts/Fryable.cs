using UnityEngine;

public class Fryable : MonoBehaviour
{
    [Header("Materiały (0-3)")]
    public Material[] materials;

    [Header("Stan usmażenia")]
    [Range(0, 3)]
    public int fryLevel = 0;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        UpdateMaterial();
    }

    // 🔥 zwiększ stan smażenia
    public void IncreaseFryLevel()
    {
        if (fryLevel < materials.Length - 1)
        {
            fryLevel++;
            UpdateMaterial();
        }
    }

    void UpdateMaterial()
    {
        if (rend != null && materials.Length > 0)
        {
            rend.material = materials[fryLevel];
        }
    }
}