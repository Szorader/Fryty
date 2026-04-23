using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Cutter : MonoBehaviour
{
    [Header("INPUT")]
    public string requiredTag = "Potato";

    [Header("OUTPUT")]
    public GameObject friesPrefab;

    [Header("OUTPUT POINT")]
    public Transform outputPoint;

    [Header("SPAWN SETTINGS")]
    public float spawnDelay = 2f;
    public float force = 5f;

    [Header("STATE")]
    public bool hasPotatoLoaded = false;
    public bool isProcessing = false;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference cutterLoopEvent;
    [SerializeField] private EventReference buttonClickEvent;

    private EventInstance cutterLoopInstance;
    private EventInstance analogButtonClickInstance;
    
    private TutorialManager tutorialManager;
    private bool tutorialActive = true;

    private bool canProcess => hasPotatoLoaded && !isProcessing;

    // WEJŚCIE ZIEMNIAKA
    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }
    public void HandleTrigger(Collider other)
    {
        if (hasPotatoLoaded) return;
        if (!other.CompareTag(requiredTag)) return;

        Destroy(other.gameObject);
        hasPotatoLoaded = true;

        Debug.Log("Potato loaded.");
    }

    // PRZYCISK
    public void PressButton(OrderDatabase.FriesType selectedType)
    {
        // Start button click audio
        RuntimeManager.PlayOneShot(buttonClickEvent, transform.position);
        
        if (!canProcess)
        {
            Debug.Log("Cutter not ready.");
            return;
        }
        
        
        
        //Start cutter audio
        cutterLoopInstance = RuntimeManager.CreateInstance(cutterLoopEvent);
        cutterLoopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        cutterLoopInstance.start();

        StartCoroutine(ProcessAndSpawn(selectedType));
    }

    private IEnumerator ProcessAndSpawn(OrderDatabase.FriesType selectedType)
    {
        isProcessing = true;

        Debug.Log("Processing: " + selectedType);

        yield return new WaitForSeconds(spawnDelay);

        Vector3 spawnPosition = outputPoint ? outputPoint.position : transform.position;

        GameObject spawned = Instantiate(friesPrefab, spawnPosition, Quaternion.identity);
        if (tutorialActive && tutorialManager.tutorialStep == 2)
        {
            tutorialManager.NextStep();
            tutorialActive = false;
        }
        
        // Stop audio 
        cutterLoopInstance.stop(STOP_MODE.ALLOWFADEOUT);
        cutterLoopInstance.release();

        FriesData friesData = spawned.GetComponent<FriesData>();
        if (friesData != null)
        {
            friesData.SetFriesType(selectedType);
        }

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = outputPoint ? outputPoint.forward : transform.forward;
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
        }

        // RESET STANU (jedno miejsce prawdy)
        hasPotatoLoaded = false;
        isProcessing = false;

        Debug.Log("Ready.");
    }
    
    // Audio cleanup
    private void OnDestroy()
    {
        cutterLoopInstance.stop(STOP_MODE.IMMEDIATE);
        cutterLoopInstance.release();
    } 
}