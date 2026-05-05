using System.Collections.Generic;
using UnityEngine;

public class UI_QueuingDevice : MonoBehaviour
{
    [Header("Clickable Buttons")]
    public List<GameObject> buttons = new List<GameObject>();

    [Header("Reference")]
    public QueuingDevice queuingDevice;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HandleClick(hit.collider);
            }
        }
    }

    private void HandleClick(Collider clickedCollider)
    {
        GameObject clicked = clickedCollider.gameObject;

        Debug.Log("[CLICK] Hit: " + clicked.name);

        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];

            if (clicked == button || clicked.transform.IsChildOf(button.transform))
            {
                Debug.Log("[UI] Button matched: " + button.name);

                if (queuingDevice == null)
                {
                    Debug.LogError("[UI] QueuingDevice is NULL");
                    return;
                }

                Debug.Log("[UI] Calling AddOrderToBasket with number: " + i);

                queuingDevice.AddOrderToBasket(i);

                return;
            }
        }

        Debug.Log("[UI] Click not in buttons list");
    }
}