using UnityEngine;

[CreateAssetMenu(fileName = "Client", menuName = "FoodTruck/Client")]
public class ClientData : ScriptableObject
{
    public string clientName;
    public GameObject clientPrefab;
    public float patienceTime;
    public bool isBadClient;
}
