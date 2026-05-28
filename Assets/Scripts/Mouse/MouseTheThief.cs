using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using System.Collections;

public class MouseTheThief : MonoBehaviour
{
    public Transform spawnPoint;
    public FriesData carriedFriesData;

    [Header("VISUALS")]
    [SerializeField] private GameObject mouseInHouse;

    [Header("Movement")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5.5f;

    [Header("GRAB")]
    [SerializeField] private float grabDuration = 0.8f;
    [SerializeField] private GameObject grabFriesVisual;

    private NavMeshAgent agent;
    private Transform currentTarget;

    private bool isGrabbing = false;
    private bool hasFry = false;

    private FriesData reservedFries;

    private Animator animator;

    [Header("AUDIO")]
    [SerializeField] private EventReference mouseRunLoop;
    [SerializeField] private EventReference mouseStartRun;
    [SerializeField] private EventReference mouseYappie;
    [SerializeField] private EventReference mouseCrunch;

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

        gameObject.SetActive(false);

        if (mouseInHouse != null)
            mouseInHouse.SetActive(true);

        if (grabFriesVisual != null)
            grabFriesVisual.SetActive(false);

        runLoopInstance = RuntimeManager.CreateInstance(mouseRunLoop);
        runLoopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void Update()
    {
        if (isGrabbing)
        {
            UpdateAudio();
            return;
        }

        // ===== RETURN WITH FRIES =====
        if (hasFry)
        {
            agent.speed = runSpeed;
            agent.SetDestination(spawnPoint.position);

            SetState(State.Running);

            if (Vector3.Distance(transform.position, spawnPoint.position) <= 0.3f)
            {
                RuntimeManager.PlayOneShot(mouseCrunch, spawnPoint.position);
                hasFry = false;
                reservedFries = null;

                if (carriedFriesData != null)
                {
                    carriedFriesData.SetFriesType(OrderDatabase.FriesType.None);
                    carriedFriesData.cookLevel = 0;
                    carriedFriesData.RefreshVisuals();
                }

                currentTarget = null;

                SetState(State.Idle);
                StopRunLoopImmediate();

                if (mouseInHouse != null)
                    mouseInHouse.SetActive(true);

                gameObject.SetActive(false);
            }

            UpdateAudio();
            return;
        }

        // ===== NORMAL BEHAVIOUR =====
        if (hasFry)
        {
            agent.speed = runSpeed;
            agent.SetDestination(spawnPoint.position);

            SetState(State.Running);

            if (Vector3.Distance(transform.position, spawnPoint.position) <= 0.3f)
            {
                hasFry = false;
                reservedFries = null;

                if (carriedFriesData != null)
                {
                    carriedFriesData.SetFriesType(OrderDatabase.FriesType.None);
                    carriedFriesData.cookLevel = 0;
                    carriedFriesData.RefreshVisuals();
                }

                currentTarget = null;

                SetState(State.Idle);
                StopRunLoopImmediate();

                if (mouseInHouse != null)
                    mouseInHouse.SetActive(true);

                gameObject.SetActive(false);
            }

            UpdateAudio();
            return;
        }
    }

    // ===== START MOVEMENT ONLY HERE =====
    public void SetTarget(Transform target)
    {
        if (hasFry) return;
        if (target == null) return;
        if (reservedFries != null) return;

        currentTarget = target;

        if (mouseInHouse != null)
            mouseInHouse.SetActive(false);

        gameObject.SetActive(true);

        RuntimeManager.PlayOneShot(mouseStartRun, transform.position);

        agent.speed = walkSpeed;
        agent.SetDestination(target.position);

        SetState(State.Walking);
    }

    public void ReturnToHome()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            180f,
            transform.eulerAngles.z
        );

        currentTarget = spawnPoint;

        agent.speed = runSpeed;
        agent.SetDestination(spawnPoint.position);

        SetState(State.Running);
    }

    // ===== GRAB =====
    private void OnTriggerEnter(Collider other)
    {
        if (hasFry) return;
        if (isGrabbing) return;

        FriesData fries = other.GetComponentInParent<FriesData>();
        if (fries == null) return;

        if (currentTarget == null) return;

        if (other.transform != currentTarget && other.transform.root != currentTarget)
            return;

        if (reservedFries != null) return;

        reservedFries = fries;

        var type = fries.friesType;
        var cook = fries.cookLevel;

        Destroy(fries.gameObject);

        StartCoroutine(GrabRoutine(type, cook));
    }

    private IEnumerator GrabRoutine(OrderDatabase.FriesType type, int cookLevel)
    {
        isGrabbing = true;

        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.updateRotation = false;

        SetState(State.Idle);

        if (animator != null)
            animator.SetBool("isGrabbing", true);

        if (grabFriesVisual != null)
        {
            grabFriesVisual.SetActive(true);

            var visual = grabFriesVisual.GetComponent<FriesData>();
            if (visual != null)
            {
                visual.SetFriesType(type);
                visual.cookLevel = cookLevel;
                visual.RefreshVisuals();
            }
        }

        yield return new WaitForSeconds(grabDuration);

        if (animator != null)
            animator.SetBool("isGrabbing", false);

        if (grabFriesVisual != null)
            grabFriesVisual.SetActive(false);

        if (carriedFriesData != null)
        {
            carriedFriesData.SetFriesType(type);
            carriedFriesData.cookLevel = cookLevel;
            carriedFriesData.RefreshVisuals();

            if (cookLevel == 1)
                RuntimeManager.PlayOneShot(mouseYappie, transform.position);
        }

        hasFry = true;
        currentTarget = spawnPoint;

        agent.updateRotation = true;
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.SetDestination(spawnPoint.position);

        SetState(State.Running);

        reservedFries = null;
        isGrabbing = false;
    }

    // ===== ANIMATION =====
    private void SetState(State state)
    {
        if (currentState == state) return;

        currentState = state;

        if (animator != null)
        {
            animator.SetBool("isWalking", state == State.Walking);
            animator.SetBool("isRunning", state == State.Running);
            animator.SetBool("isIdleing", state == State.Idle);
        }
    }

    // ===== AUDIO =====
    private bool IsActuallyMoving()
    {
        if (agent == null) return false;
        if (!agent.enabled) return false;
        if (!agent.isOnNavMesh) return false;

        return agent.velocity.magnitude > 0.1f;
    }

    private void UpdateAudio()
    {
        if (runLoopInstance.isValid())
            runLoopInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        bool isMoving = IsActuallyMoving();

        if (isMoving && !isRunLoopPlaying)
        {
            runLoopInstance.start();
            isRunLoopPlaying = true;
        }

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