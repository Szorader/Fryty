using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public OrderManager orderManager;
    public ServingOrderManager servingOrderManager;

    void Update()
    {
        // spawn klienta
        if (Input.GetKeyDown(KeyCode.Z))
        {
            spawnManager.SpawnClient();
        }

        // przyjęcie zamówienia
        if (Input.GetKeyDown(KeyCode.X))
        {
            orderManager.TakeOrder();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            servingOrderManager.ServeNextClient();
        }
    }
}