using UnityEngine;

public class HideOnClick : MonoBehaviour
{
    [Header("Obiekt do schowania")]
    public GameObject objectToHide;

    public bool IsCorect = false;

    private void OnMouseDown()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
            IsCorect = true;
        }
    }
}