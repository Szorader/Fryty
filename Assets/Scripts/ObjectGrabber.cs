/*using UnityEngine;

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
}*/
using UnityEngine;
using System.Collections.Generic;

public class ObjectGrabberLoose : MonoBehaviour
{
    public float grabDistance = 3f;
    public float holdDistance = 2f;

    public float moveForce = 300f;
    public float damping = 15f;

    public float grabRadius = 2f; // 🔥 promień zbierania obiektów

    private List<Rigidbody> grabbedObjects = new List<Rigidbody>();
    private Dictionary<Rigidbody, Vector3> grabOffsets = new Dictionary<Rigidbody, Vector3>();

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
        foreach (var rb in grabbedObjects)
        {
            MoveObject(rb);
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            // 🔥 znajdź obiekty w promieniu
            Collider[] colliders = Physics.OverlapSphere(hit.point, grabRadius);

            grabbedObjects.Clear();
            grabOffsets.Clear();

            foreach (Collider col in colliders)
            {
                if (col.attachedRigidbody != null)
                {
                    Rigidbody rb = col.attachedRigidbody;

                    if (!grabbedObjects.Contains(rb))
                    {
                        grabbedObjects.Add(rb);

                        // zapamiętaj offset dla każdego obiektu
                        Vector3 localOffset = rb.transform.InverseTransformPoint(hit.point);
                        grabOffsets.Add(rb, localOffset);

                        rb.useGravity = true;
                    }
                }
            }
        }
    }

    void Release()
    {
        grabbedObjects.Clear();
        grabOffsets.Clear();
    }

    void MoveObject(Rigidbody rb)
    {
        if (!grabOffsets.ContainsKey(rb)) return;

        Vector3 grabOffset = grabOffsets[rb];

        Vector3 worldGrabPoint = rb.transform.TransformPoint(grabOffset);
        Vector3 toTarget = holdPoint.position - worldGrabPoint;

        Vector3 force = toTarget * moveForce;

        Vector3 pointVelocity = rb.GetPointVelocity(worldGrabPoint);
        Vector3 dampingForce = -pointVelocity * damping;

        rb.AddForceAtPosition(force + dampingForce, worldGrabPoint);
    }
}