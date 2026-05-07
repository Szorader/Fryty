using UnityEngine;
using FMODUnity;
using System.Collections;
using Unity.VisualScripting;
/// <summary>
/// Policeman entry inside foodtruck
/// </summary>
public class Endgame : MonoBehaviour
{
    [SerializeField] private EventReference firstKnock;
    [SerializeField] private EventReference secondKnock;
    [SerializeField] private EventReference doorKickDown;
    
    // RuntimeManager.PlayOneShot(firstKnock, transform.position);
    // RuntimeManager.PlayOneShot(secondKnock, transform.position);
    // RuntimeManager.PlayOneShot(doorKickDown, transform.position);
    
    
    public float startDelay = 2f;
    public float pauseBetweenKnocks = 3f;
    public float pauseAfterKnocks = 3f;

    
    public MonoBehaviour playerMovementScript;

    
    public Camera playerCamera;
    public Transform lookTarget;
    public float cameraLookSpeed = 2f;

    
    public Animator animator;
    public string animationTrigger = "StartAnimation";
    
    public Transform spawnPoint;
    public GameObject policeman;

    private bool isLooking = false;
    /*public void Start()
    {
        StartCoroutine(EventSequence());
    }*/
    public void StartAnimation()
    {
        StartCoroutine(EventSequence());
    }
     IEnumerator EventSequence()
    {
        Debug.Log("1");
        yield return new WaitForSecondsRealtime(startDelay);
        Debug.Log("2");
        // 1 knock knock
        RuntimeManager.PlayOneShot(firstKnock, transform.position);

        Debug.Log("3");
        yield return new WaitForSecondsRealtime(pauseBetweenKnocks);

        Debug.Log("4");
        // 2 knock knock
        RuntimeManager.PlayOneShot(secondKnock, transform.position);

        Debug.Log("5");
        yield return new WaitForSecondsRealtime(pauseAfterKnocks);
        
        Debug.Log("6");
        // door kick
        RuntimeManager.PlayOneShot(doorKickDown, transform.position);
        playerMovementScript.enabled = false;
        
        isLooking = true;
        
        Debug.Log("7");
        animator.SetTrigger(animationTrigger);
    }

    void Update()
    {
        if (isLooking)
        {
            LookAtTarget();
        }
    }

    void LookAtTarget()
    {
        Vector3 direction = lookTarget.position - playerCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerCamera.transform.rotation = Quaternion.Slerp(
            playerCamera.transform.rotation,
            targetRotation,
            cameraLookSpeed * Time.deltaTime
        );
    }
}
