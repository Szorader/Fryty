using UnityEngine;

[CreateAssetMenu(fileName = "Client", menuName = "Client/Create Client")]
public class ClientData : ScriptableObject
{
    public GameObject clientPrefab;

    [Header("Body Materials")]
    public Material[] goodMaterials;
    public Material[] badMaterials;

    [Header("Face Materials")]
    public Material goodFaceMaterial;
    public Material badFaceMaterial;
    
    [Header("Voice")]
    public int[] availableVoiceActors;
}
