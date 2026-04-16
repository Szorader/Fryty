using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    public GameObject root; // cały panel (do włączania/wyłączania)
    public TMP_Text actionText;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string text)
    {
        root.SetActive(true);
        actionText.text = text;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}