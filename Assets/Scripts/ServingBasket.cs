using UnityEngine;

public class ServingBasket : MonoBehaviour
{
    [Header("Frytka")]
    [SerializeField] private GameObject fryInBasket;

    public bool isReady = false;

    private void Start()
    {
        
        // Na start ukryj frytkę w koszyku
        RefreshBasket();
    }

    private void RefreshBasket()
    {
        if (fryInBasket != null)
        {
            fryInBasket.SetActive(false);
            isReady = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriedFry"))
        {
            // Pokazuje frytkę w koszyku
            if (fryInBasket != null)
            {
                fryInBasket.SetActive(true);
                Destroy(other.gameObject);
                isReady=true;
            }

            // Usuwa przeciąganą frytkę
            //
        }
    }
    public bool isCorrect;

    public void Serve(ClientController client)
    {
        if (!isReady)
        {
            Debug.Log("Nie przygotowany posiłek");
            return;
        }
        if (client == null)
        {
            Debug.Log("Brak klienta");
            return;
        }

        // zły klient
        if (client.clientData.data.isBadClient)
        {
            Debug.Log("klient był tajnym agentem nie powinien dostać zamówienia");
            RefreshBasket();
            return;
        }

        // normalna obsługa
        if (isCorrect)
        {
            Debug.Log(client.clientData.data.clientName + " dostał dobre zamówienie");
        }
        else
        {
            Debug.Log(client.clientData.data.clientName + " dostał złe zamówienie");
        }
        RefreshBasket();
        
    }
}