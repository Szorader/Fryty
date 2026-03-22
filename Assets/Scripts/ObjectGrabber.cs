using UnityEngine;

public class ObjectGrabberLoose : MonoBehaviour
{
    public float grabDistance = 3f;
    public float holdDistance = 2f;

    public float moveForce = 300f;
    public float damping = 15f;

    private Rigidbody grabbedObject;
    private Vector3 grabOffset; // punkt złapania względem obiektu
    private Transform holdPoint;

    void Start()
    {
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.parent = transform;
        holdPoint.localPosition = new Vector3(0, 0, holdDistance);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrab();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }

    void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            MoveObject();
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            if (hit.rigidbody != null)
            {
                grabbedObject = hit.rigidbody;

                // zapamiętaj dokładne miejsce złapania
                grabOffset = grabbedObject.transform.InverseTransformPoint(hit.point);

                grabbedObject.useGravity = true; // zostawiamy grawitację!
            }
        }
    }

    void Release()
    {
        grabbedObject = null;
    }

    void MoveObject()
    {
        // aktualna pozycja punktu złapania w świecie
        Vector3 worldGrabPoint = grabbedObject.transform.TransformPoint(grabOffset);

        // kierunek do punktu trzymania
        Vector3 toTarget = holdPoint.position - worldGrabPoint;

        // siła "ciągnięcia" za konkretny punkt
        Vector3 force = toTarget * moveForce;

        // tłumienie w tym punkcie
        Vector3 pointVelocity = grabbedObject.GetPointVelocity(worldGrabPoint);
        Vector3 dampingForce = -pointVelocity * damping;

        // dodaj siłę w miejscu złapania (KLUCZ!)
        grabbedObject.AddForceAtPosition(force + dampingForce, worldGrabPoint);
    }
}