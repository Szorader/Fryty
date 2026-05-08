using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class MouseTheThief : MonoBehaviour
{
    public Transform spawnPoint;
    public FriesData carriedFriesData;

    [Header("Movement")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5.5f;

    private NavMeshAgent agent;
    private Transform currentTarget;

    private bool hasFry = false;

    private Animator animator;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference mouseRunLoop;
    [SerializeField] private EventReference mouseStartRun;

    private FMOD.Studio.EventInstance runLoopInstance;
    private bool isRunLoopPlaying = false;
    
    private enum State
    {
        Idle,
        Walking,
        Running
    }

    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;

        // startowo mysz w domu = niewidoczna
        gameObject.SetActive(false);
        
        // audio 
        runLoopInstance = RuntimeManager.CreateInstance(mouseRunLoop);
        runLoopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void Update()
    {
        if (hasFry)
        {
            agent.speed = runSpeed;
            agent.SetDestination(spawnPoint.position);

            SetState(State.Running);

            float dist = Vector3.Distance(transform.position, spawnPoint.position);

            if (dist <= 0.3f)
            {
                hasFry = false;

                agent.speed = walkSpeed;

                if (carriedFriesData != null)
                {
                    carriedFriesData.SetFriesType(OrderDatabase.FriesType.None);
                    carriedFriesData.cookLevel = 0;
                    carriedFriesData.RefreshVisuals();
                }

                currentTarget = null;

                SetState(State.Idle);

                // stop audio before hiding
                StopRunLoopImmediate();

                // hide mouse
                gameObject.SetActive(false);
            }

            UpdateAudio();
            return;
        }

        // target exists -> move
        if (currentTarget != null)
        {
            // fries disappeared before mouse got them
            if (currentTarget.gameObject == null || !currentTarget.gameObject.activeInHierarchy)
            {
                ReturnToHome();
            }
            else
            {
                agent.speed = walkSpeed;
                agent.SetDestination(currentTarget.position);

                SetState(State.Walking);
            }
        }
        else
        {
            SetState(State.Idle);
        }

        UpdateAudio();
    }

    public void SetTarget(Transform target)
    {
        if (hasFry) return;

        currentTarget = target;

        // POJAWIA SIĘ GDY JEST FRYTKA
        gameObject.SetActive(true);
        
        // play audio
        RuntimeManager.PlayOneShot(mouseStartRun, transform.position);

        agent.speed = walkSpeed;
        agent.SetDestination(target.position);

        SetState(State.Walking);
    }

    public void ReturnToHome()
    {
        currentTarget = spawnPoint;

        agent.speed = runSpeed;
        agent.SetDestination(spawnPoint.position);

        SetState(State.Running);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFry) return;

        FriesData fries = other.GetComponentInParent<FriesData>();
        if (fries == null) return;

        if (currentTarget == null) return;

        if (other.transform != currentTarget && other.transform.root != currentTarget)
            return;

        if (carriedFriesData != null)
        {
            carriedFriesData.SetFriesType(fries.friesType);
            carriedFriesData.cookLevel = fries.cookLevel;
            carriedFriesData.RefreshVisuals();
        }

        hasFry = true;

        agent.speed = runSpeed;

        currentTarget = spawnPoint;
        agent.SetDestination(spawnPoint.position);

        Destroy(fries.gameObject);

        SetState(State.Running);
    }

    private void SetState(State state)
    {
        if (currentState == state) return;

        State previousState = currentState;
        currentState = state;

        if (animator != null)
        {
            animator.SetBool("isWalking", state == State.Walking);
            animator.SetBool("isRunning", state == State.Running);
            animator.SetBool("isIdleing", state == State.Idle);
        }

    }
    
    // =======================
    // Audio logic
    // =======================
    
    private bool IsActuallyMoving()
    {
        if (agent == null) return false;

        if (!agent.enabled) return false;

        if (!agent.isOnNavMesh) return false;

        return agent.velocity.magnitude > 0.1f;
    }

    private void UpdateAudio()
    {
        // update 3D position
        if (runLoopInstance.isValid())
        {
            runLoopInstance.set3DAttributes(
                RuntimeUtils.To3DAttributes(gameObject)
            );
        }

        bool isMoving = IsActuallyMoving();

        // start loop
        if (isMoving && !isRunLoopPlaying)
        {
            runLoopInstance.start();
            isRunLoopPlaying = true;
        }

        // stop loop
        if (!isMoving && isRunLoopPlaying)
        {
            runLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isRunLoopPlaying = false;
        }
    }

    private void StopRunLoopImmediate()
    {
        if (!isRunLoopPlaying) return;

        runLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        isRunLoopPlaying = false;
    }

    private void OnDestroy()
    {
        if (runLoopInstance.isValid())
        {
            runLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            runLoopInstance.release();
        }
    }
}