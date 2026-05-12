using UnityEngine;

public class Broom : MonoBehaviour
{
    [SerializeField] private Transform holdSlot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;

    private bool isHeld = false;

    private Rigidbody rb;
    private Collider col;

    public bool IsHeld => isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isHeld)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryClean();
        }
    }

    private void OnMouseDown()
    {
        if (isHeld)
            return;

        Pickup();
    }

    private void Pickup()
    {
        isHeld = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (col != null)
            col.enabled = false;

        transform.SetParent(holdSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        isHeld = false;

        transform.SetParent(null);

        if (col != null)
            col.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(holdSlot.forward * 2f, ForceMode.Impulse);
        }
    }

    private void TryClean()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                hit.collider.gameObject.SetActive(false);
            }
        }
    }
}