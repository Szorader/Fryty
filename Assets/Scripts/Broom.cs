using UnityEngine;
using FMODUnity;
using TMPro;
public class Broom : MonoBehaviour
{
    [SerializeField] private Transform holdSlot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private GameObject actionObject;
    [SerializeField] public TMP_Text actionText;

    private bool isHeld = false;

    private Rigidbody rb;
    private Collider col;

    public bool IsHeld => isHeld;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference sweepAudio;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem cleanParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        actionObject.SetActive(false);
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

        actionObject.SetActive(true);
        actionText.text = "LMB - Clean " + "Q - Drop broom";
            
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Wyłącz wszystkie collidery
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        transform.SetParent(holdSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        
        isHeld = false;
        
        actionObject.SetActive(false);
        
        transform.SetParent(null);

        // Włącz wszystkie collidery
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerCamera.transform.forward * 3f, ForceMode.Impulse);
        }
    }

    private void TryClean()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                // play audio
                RuntimeManager.PlayOneShot(sweepAudio, transform.position);
                hit.collider.gameObject.SetActive(false);
                
                // Spawn particles at trash position
                if (cleanParticles != null)
                {
                    ParticleSystem particles =
                        Instantiate(
                            cleanParticles,
                            hit.collider.transform.position,
                            Quaternion.identity
                        );

                    Destroy(
                        particles.gameObject,
                        particles.main.duration +
                        particles.main.startLifetime.constantMax
                    );
                }
            }
        }
    }
}