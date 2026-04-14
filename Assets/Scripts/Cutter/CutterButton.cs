using UnityEngine;

public class CutterButton : MonoBehaviour
{
    public OrderDatabase.FriesType friesType;

    private Cutter parentCutter;

    private void Awake()
    {
        parentCutter = GetComponentInParent<Cutter>();

        if (!parentCutter)
        {
            Debug.LogWarning($"CutterButton on {gameObject.name} has no Cutter in parents.");
        }
    }

    public void PressButton()
    {
        if (!parentCutter) return;

        parentCutter.PressButton(friesType);
    }

    private void OnMouseDown()
    {
        PressButton();
    }
}