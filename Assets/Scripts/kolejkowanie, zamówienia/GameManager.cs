using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public SpawnManager spawnManager;
    //public QueueManager queueManager;
    //pozniej to wszystko zostanie usuniete bo bedzie to robione automatycznie przez gre
    public DayManager dayManager;

    private void Start()
    {
        dayManager = FindObjectOfType<DayManager>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }-/
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            dayManager.WrongKill();
        }*/
        // spawn klienta
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            spawnManager.SpawnClient();
        }*/

        // przyjęcie zamówienia
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            queueManager.TakeOrder();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            queueManager.ServeNextClient();
        }
*/
        /* if (Input.GetKeyDown(KeyCode.V))
         {
             queueManager.AddOrderToBasket();
         }*/
    }
}