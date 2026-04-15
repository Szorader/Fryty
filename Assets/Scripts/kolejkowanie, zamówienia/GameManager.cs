using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public QueueManager queueManager;
    //pozniej to wszystko zostanie usuniete bo bedzie to robione automatycznie przez gre
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
            queueManager.TakeOrder();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            queueManager.ServeNextClient();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            queueManager.AddOrderToBasket();
        }
    }
}