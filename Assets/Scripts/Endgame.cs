using UnityEngine;
using FMODUnity;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// Policeman entry inside foodtruck
/// </summary>
public class Endgame : MonoBehaviour
{
    [SerializeField] private EventReference firstKnock;
    [SerializeField] private EventReference secondKnock;
    [SerializeField] private EventReference doorKickDown;
    [SerializeField] private EventReference copTalk;
    
    [SerializeField] private DeathScreenManager deathScreen;
    
    // RuntimeManager.PlayOneShot(firstKnock, transform.position);
    // RuntimeManager.PlayOneShot(secondKnock, transform.position);
    // RuntimeManager.PlayOneShot(doorKickDown, transform.position);
    
    
    public float startDelay = 2f;
    public float pauseBetweenKnocks = 3f;
    public float pauseAfterKnocks = 3f;
    public float pauseBeforeTalk = 0.8f;

    
    public MonoBehaviour playerMovementScript;

    
    public Camera playerCamera;
    public Transform lookTarget;
    public float cameraLookSpeed = 2f;

    
    public Animator animator;
    public string animationTrigger = "StartAnimation";
    
    public Transform spawnPoint;
    public GameObject policeman;

    private bool sequenceStarted = false;
    private bool isLooking = false;
    
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;
    /*public void Start()
    {
        StartCoroutine(EventSequence());
    }*/
    public void StartAnimation()
    {
        if (sequenceStarted) return;

        sequenceStarted = true;
        StartCoroutine(EventSequence());
    }
     IEnumerator EventSequence()
    {
        
        
        yield return new WaitForSecondsRealtime(startDelay);
        
        // 1 knock knock
        RuntimeManager.PlayOneShot(firstKnock, transform.position);

        
        yield return new WaitForSecondsRealtime(pauseBetweenKnocks);

        
        // 2 knock knock
        RuntimeManager.PlayOneShot(secondKnock, transform.position);

        //spawn policeman
        GameObject obj = Instantiate(policeman, spawnPoint.position, spawnPoint.rotation);
        
        yield return new WaitForSecondsRealtime(pauseAfterKnocks);
        
        
        // door kick
        RuntimeManager.PlayOneShot(doorKickDown, transform.position);
        
        
        
        var player = playerMovementScript.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.Die(); // stops footsteps audio + locks death state + unlocks cursor
        }
        playerMovementScript.enabled = false;
        
        // cop talks
                yield return new WaitForSecondsRealtime(pauseBeforeTalk);
                RuntimeManager.PlayOneShot(copTalk, transform.position);
        
        isLooking = true;
        
        animator.SetTrigger(animationTrigger);
        
        deathScreen.ShowArrest(playerMovementScript.gameObject);
        yield return new WaitForSecondsRealtime(5f);
        
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
