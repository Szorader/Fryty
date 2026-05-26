using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class UI_QueuingDevice : MonoBehaviour
{
    [Header("Clickable Buttons")]
    public List<GameObject> buttons = new List<GameObject>();

    [Header("Reference")]
    public QueuingDevice queuingDevice;

    public bool canGiveOrder = false;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference pikacz;
    
    void Update()
    {
        Debug.Log(canGiveOrder);
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
        {
            return;
        }
            
        GameObject clicked = clickedCollider.gameObject;
        

        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];

            if (clicked == button || clicked.transform.IsChildOf(button.transform))
            {
                if (queuingDevice == null)
                {
                    return;
                }

                // audio 
                RuntimeManager.PlayOneShot(
                    pikacz,
                    button.transform.position
                );
                
                
                queuingDevice.AddOrderToBasket(i);

                return;
            }
        }
    }
}