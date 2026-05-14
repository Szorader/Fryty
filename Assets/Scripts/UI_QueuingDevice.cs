using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class UI_QueuingDevice : MonoBehaviour
{
    [Header("Clickable Buttons")]
    public List<GameObject> buttons = new List<GameObject>();

    [Header("Reference")]
    public QueuingDevice queuingDevice;

    public bool canGiveOrder = true;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference pikacz;

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
        if (!canGiveOrder)
            return;
        GameObject clicked = clickedCollider.gameObject;

        //Debug.Log("[CLICK] Hit: " + clicked.name);

        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];

            if (clicked == button || clicked.transform.IsChildOf(button.transform))
            {
                //Debug.Log("[UI] Button matched: " + button.name);

                if (queuingDevice == null)
                {
                    //Debug.LogError("[UI] QueuingDevice is NULL");
                    return;
                }

                //Debug.Log("[UI] Calling AddOrderToBasket with number: " + i);

                // audio 
                RuntimeManager.PlayOneShot(
                    pikacz,
                    button.transform.position
                );
                
                canGiveOrder = false;
                queuingDevice.AddOrderToBasket(i);

                return;
            }
        }

        //Debug.Log("[UI] Click not in buttons list");
    }
}