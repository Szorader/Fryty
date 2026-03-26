using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "FoodTruck/Order")]
public class OrderData : ScriptableObject
{
    public string orderName;
    public string[] ingredients;
}