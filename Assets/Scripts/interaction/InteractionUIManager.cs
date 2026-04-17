using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    public GameObject root; // cały panel (do włączania/wyłączania)
    public TMP_Text actionText;
    
    public Image crosshair;
    public Image interactableCrosshair;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string text)
    {
        crosshair.enabled = false;
        interactableCrosshair.enabled = true;
        root.SetActive(true);
        actionText.text = text;
    }

    public void Hide()
    {
        crosshair.enabled = true;
        interactableCrosshair.enabled = false;
        root.SetActive(false);
    }
}