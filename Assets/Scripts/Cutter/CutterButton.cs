using UnityEngine;

public class CutterButton : MonoBehaviour
{
    public FriesData.FriesType friesType;
    private Cutter parentCutter;

    private void Start()
    {
        parentCutter = GetComponentInParent<Cutter>();
    }

    public void PressButton()
    {
        if (parentCutter != null)
        {
            parentCutter.PressButton(friesType);
        }
    }

    private void OnMouseDown()
    {
        PressButton();
    }
}