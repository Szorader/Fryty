using UnityEngine;

[CreateAssetMenu(fileName = "Client", menuName = "Client/Create Client")]
public class ClientData : ScriptableObject
{
    public GameObject clientPrefab;
    //public bool isBadClient;
    [Header("Materials")]
    public Material[] goodMaterials;
    public Material[] badMaterials;
}
