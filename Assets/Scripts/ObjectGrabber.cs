using UnityEngine;
using System.Collections.Generic;

public class ObjectGrabberLoose : MonoBehaviour
{
    public float grabDistance = 3f;
    public float holdDistance = 2f;

    public float moveForce = 300f;
    public float damping = 15f;

    // 🔹 Lista tagów przez które można łapać
    public List<string> grabbableTags = new List<string>();

    private HashSet<string> tagSet;

    private Rigidbody grabbedObject;
    private Vector3 grabOffset;
    private Transform holdPoint;

    void Awake()
    {
        // zamieniamy listę na HashSet dla wydajności
        tagSet = new HashSet<string>(grabbableTags);
    }

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
            // 🔹 SPRAWDZANIE TAGU COLLIDERA
            if (hit.rigidbody != null && tagSet.Contains(hit.collider.tag))
            {
                grabbedObject = hit.rigidbody;

                grabOffset = grabbedObject.transform.InverseTransformPoint(hit.point);

                grabbedObject.useGravity = true;
            }
        }
    }

    void Release()
    {
        grabbedObject = null;
    }

    void MoveObject()
    {
        Vector3 worldGrabPoint = grabbedObject.transform.TransformPoint(grabOffset);

        Vector3 toTarget = holdPoint.position - worldGrabPoint;

        Vector3 force = toTarget * moveForce;

        Vector3 pointVelocity = grabbedObject.GetPointVelocity(worldGrabPoint);
        Vector3 dampingForce = -pointVelocity * damping;

        grabbedObject.AddForceAtPosition(force + dampingForce, worldGrabPoint);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward * grabDistance;

        Gizmos.DrawLine(origin, origin + direction);
        Gizmos.DrawWireSphere(origin + direction, 0.2f);

        if (Application.isPlaying && holdPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPoint.position, 0.2f);
        }
    }
}
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward * grabDistance;

        Gizmos.DrawLine(origin, origin + direction);
        Gizmos.DrawWireSphere(origin + direction, 0.2f);

        // hold point
        if (Application.isPlaying && holdPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPoint.position, 0.2f);
        }
    }
}*/

/*using UnityEngine;
using System.Collections.Generic;

public class ObjectGrabberLoose : MonoBehaviour
{
    public float grabDistance = 3f;
    public float holdDistance = 2f;

    public float moveSpeed = 10f; // 🔥 zamiast moveForce
    public float grabRadius = 2f;
    public float spacing = 0.5f;

    private List<Rigidbody> grabbedObjects = new List<Rigidbody>();
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
            TryGrab();

        if (Input.GetMouseButtonUp(0))
            Release();
    }

    void FixedUpdate()
    {
        // 🔥 przeniesione z Update (synchronizacja z fizyką)
        holdPoint.localPosition = new Vector3(0, 0, holdDistance);

        for (int i = 0; i < grabbedObjects.Count; i++)
        {
            MoveObject(grabbedObjects[i], i);
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, grabRadius);

            grabbedObjects.Clear();

            foreach (Collider col in colliders)
            {
                if (col.attachedRigidbody != null)
                {
                    Rigidbody rb = col.attachedRigidbody;

                    if (!grabbedObjects.Contains(rb))
                    {
                        grabbedObjects.Add(rb);

                        rb.useGravity = true;

                        // 🔥 FIX 1: płynność ruchu
                        rb.interpolation = RigidbodyInterpolation.Interpolate;

                        // 🔥 FIX 2: lepsze kolizje przy ruchu
                        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    }
                }
            }
        }
    }

    void Release()
    {
        grabbedObjects.Clear();
    }

    void MoveObject(Rigidbody rb, int index)
    {
        Vector3 offset = GetFormationOffset(index);
        Vector3 targetPos = holdPoint.position + offset;

        Vector3 toTarget = targetPos - rb.position;

        // 🔥 FIX 3: zamiast AddForce → velocity (mega smooth)
        rb.linearVelocity = toTarget * moveSpeed;

        // 🔥 FIX 4: redukcja drgań rotacji
        rb.angularVelocity *= 0.8f;
    }

    Vector3 GetFormationOffset(int index)
    {
        float angle = index * 137.5f * Mathf.Deg2Rad; // 🔥 poprawka (radiany!)
        float radius = spacing * Mathf.Sqrt(index);

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        return new Vector3(x, y, 0);
    }
}*/