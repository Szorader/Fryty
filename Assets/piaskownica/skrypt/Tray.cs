using UnityEngine;

public class Tray : MonoBehaviour
{
    public bool isCorrect;

    public void Serve(ClientController client)
    {
        if (client == null)
        {
            Debug.Log("Brak klienta");
            return;
        }

        // 🔴 NOWA LOGIKA — zły klient
        if (client.clientData.data.isBadClient)
        {
            Debug.Log("❌ klient był tajnym agentem nie powinien dostać zamówienia");
            return;
        }

        // 🟢 normalna obsługa
        if (isCorrect)
        {
            Debug.Log(client.clientData.data.clientName + " dostał dobre zamówienie 😄");
        }
        else
        {
            Debug.Log(client.clientData.data.clientName + " dostał złe zamówienie 😡");
        }
    }
}