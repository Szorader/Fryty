
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float castDistance = 3f;
    //public Vector3 raycastOffset = new Vector3(0f, 0f, 0f);
    private IInteractable currentInteractable;
    [Header("Debug")]
    public bool showRaycast = true;
    public Color rayColor = Color.red;

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (DoInteractionTest(out IInteractable interactable))
            {
                if (interactable.CanInteract())
                {
                    interactable.Interact(this);
                }
            }
        }
    }*/
    private void Update()
    {
        if (DoInteractionTest(out IInteractable interactable))
        {
            bool canInteract = interactable.CanInteract();

            if (interactable != currentInteractable)
            {
                HideCurrent();
                currentInteractable = interactable;
            }

            if (canInteract)
            {
                ShowCurrent();
            }
            else
            {
                HideCurrent();
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                interactable.Interact(this);
            }
        }
        else
        {
            HideCurrent();
            currentInteractable = null;
        }
    }
    /*private void Update()
    {
        if (DoInteractionTest(out IInteractable interactable))
        {
            if (interactable != currentInteractable)
            {
                HideCurrent();
                currentInteractable = interactable;
                ShowCurrent();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interactable.CanInteract())
                {
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            HideCurrent();
            currentInteractable = null;
        }
    }*/
    /*private void ShowCurrent()
    {
        if (currentInteractable is MonoBehaviour mb)
        {
            var prompt = mb.GetComponentInChildren<Canvas>();
            if (prompt != null)
                prompt.enabled = true;
        }
    }

    private void HideCurrent()
    {
        if (currentInteractable is MonoBehaviour mb)
        {
            var prompt = mb.GetComponentInChildren<Canvas>(true);
            if (prompt != null)
                prompt.enabled = false;
        }
    }*/
    private void ShowCurrent()
    {
        if (currentInteractable != null)
        {
            InteractionUIManager.Instance.Show(currentInteractable.GetPrompt());
        }
    }

    private void HideCurrent()
    {
        InteractionUIManager.Instance.Hide();
    }

    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;

        Ray ray = new Ray(transform.position /*+ raycastOffset*/, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance))
        {
            interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!showRaycast) return;

        Gizmos.color = rayColor;

        Vector3 origin = transform.position /*+ raycastOffset*/;
        Vector3 direction = transform.forward * castDistance;

        // linia raycasta
        Gizmos.DrawLine(origin, origin + direction);

        // kulka na końcu (łatwiej zobaczyć zasięg)
        Gizmos.DrawSphere(origin + direction, 0.1f);
    }
}
