using UnityEngine;
using UnityEngine.AI;

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

                // 🔥 WRACA DO DOMU → ZNIKA
                gameObject.SetActive(false);
            }

            return;
        }

        if (currentTarget != null)
        {
            agent.speed = walkSpeed;
            agent.SetDestination(currentTarget.position);
            SetState(State.Walking);
        }
        else
        {
            SetState(State.Idle);
        }
    }

    public void SetTarget(Transform target)
    {
        if (hasFry) return;

        currentTarget = target;

        // 🔥 POJAWIA SIĘ GDY JEST FRYTKA
        gameObject.SetActive(true);

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
        currentState = state;

        if (animator == null) return;

        animator.SetBool("isWalking", state == State.Walking);
        animator.SetBool("isRunning", state == State.Running);
        animator.SetBool("isIdleing", state == State.Idle);
    }
}