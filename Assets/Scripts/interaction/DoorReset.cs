using UnityEngine;

public class DoorReset : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    //private HingeJoint hinge;

    private bool locked = false;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        //hinge = GetComponent<HingeJoint>();
    }

    
    // Przywraca drzwi do pozycji i rotacji początkowej.
    
    public void ResetDoor()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.Sleep();
    }

    
    // Blokuje drzwi.
    
    public void LockDoor()
    {
        locked = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    
    // Odblokowuje drzwi.
    
    public void UnlockDoor()
    {
        locked = false;

        rb.constraints = RigidbodyConstraints.None;
    }
    
}