using UnityEngine;

public class SpawnOrderTicket : MonoBehaviour
{
    [Header("UI")]
    public GameObject ticketPrefab;

    private int currentOrderNumber = 1;

    public void SpawnTicket(CustomerOrder order)
    {
        // 🔥 spawn w miejscu obiektu tego skryptu
        GameObject ticket = Instantiate(
            ticketPrefab,
            transform.position,
            transform.rotation
        );

        OrderTicketUI ui = ticket.GetComponent<OrderTicketUI>();

        if (ui == null)
        {
            Debug.LogError("Brak OrderTicketUI na prefabie!");
            return;
        }

        ui.SetOrder(
            order.clientName,
            currentOrderNumber,
            order.fries,
            order.sauce,
            order.seasoning
        );

        // opcjonalny obrót wizualny
        ticket.transform.rotation = Quaternion.Euler(0f, 180f, 10f);

        currentOrderNumber++;
    }
}