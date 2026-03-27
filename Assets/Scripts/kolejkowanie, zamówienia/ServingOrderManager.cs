using UnityEngine;

public class ServingOrderManager: MonoBehaviour
{
    public QueueManager queueManager; // Twoja kolejka
    //public Tray tray; // Tacka
    public ServingBasket servingBasket;
    
    public void ServeNextClient()
    {
        // pobierz pierwszego klienta z kolejki odbioru
        ClientController client = queueManager.GetFirstPickupClient();

        if (client == null)
        {
            Debug.Log("Brak klientów do obsługi");
            return;
        }

        // wywołujemy tackę
        
        //tray.Serve(client);
        servingBasket.Serve(client);
        queueManager.RemoveClient(client);
    }
}