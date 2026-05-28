using System.Collections;
using UnityEngine;
using FMODUnity;
using TMPro;

public class Broom : MonoBehaviour
{
    [Header("Hold")]
    [SerializeField] private Transform holdSlot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;

    [Header("UI")]
    [SerializeField] private GameObject actionObject;
    [SerializeField] public TMP_Text actionText;

    [Header("ANIMATION")]
    [SerializeField] private Animator pocketAnimator;

    [Header("STATS")]
    [SerializeField] private Wallet wallet;
    public int cleanedTrashCount = 0;

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

        if (actionObject != null)
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

        if (actionObject != null)
        {
            actionObject.SetActive(true);
            actionText.text = "LMB - Clean\nQ - Drop broom";
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

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

        if (actionObject != null)
            actionObject.SetActive(false);

        transform.SetParent(null);

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
                RuntimeManager.PlayOneShot(sweepAudio, transform.position);

                hit.collider.gameObject.SetActive(false);

                // MONEY
                if (wallet != null)
                {
                    wallet.EarnMoney(1f);
                }

                // STATS + SAVE
                cleanedTrashCount++;

                if (SaveSystem.Instance != null)
                {
                    SaveSystem.Instance.saveData.cleanedTrashCount = cleanedTrashCount;
                }

                // ANIMATION
                if (pocketAnimator != null)
                {
                    StartCoroutine(PlayBroomAnim());
                }

                // VFX
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

    private IEnumerator PlayBroomAnim()
    {
        pocketAnimator.SetBool("isBrooming", true);

        yield return new WaitForSeconds(1f);

        pocketAnimator.SetBool("isBrooming", false);
    }
}