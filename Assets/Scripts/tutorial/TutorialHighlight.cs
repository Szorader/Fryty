using UnityEngine;

public class TutorialHighlight : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    private MaterialPropertyBlock block;
    [SerializeField] private Renderer rend;

    private static readonly int HighlightID =
        Shader.PropertyToID("_TutorialHighlight");
    
    private void Awake()
    {
        block = new MaterialPropertyBlock();

        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        SetHighlight(false);
    }

    public void SetHighlight(bool enabled)
    {
        foreach (Renderer r in renderers)
        {
            if (r == null)
                continue;

            r.material.SetFloat(
                "_TutorialHighlight",
                enabled ? 1f : 0f
            );
        }
    }
}
