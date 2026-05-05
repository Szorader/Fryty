using UnityEngine;
using UnityEngine.UI;

public class ButtonDebug : MonoBehaviour
{
    public void Pressed()
    {
        Debug.Log("Przycisk został naciśnięty! ✅");
        
        // Możesz dodać więcej informacji:
        // Debug.Log("Czas: " + Time.time);
        // Debug.LogWarning("To jest warning!");
        // Debug.LogError("To jest error!");
    }
}