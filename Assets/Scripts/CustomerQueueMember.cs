using UnityEngine;

public class CustomerQueueMember : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arriveDistance = 0.05f;

    private CustomerQueueManager queueManager;
    private Vector3 targetPosition;
    private string lastKnownTag;
    private bool goingToExit = false;

    public void Initialize(CustomerQueueManager manager)
    {
        queueManager = manager;
        lastKnownTag = gameObject.tag;
        goingToExit = false;
    }

    private void Start()
    {
        lastKnownTag = gameObject.tag;
    }

    private void Update()
    {
        MoveToTarget();
        DetectTagChange();
        CheckIfReachedExit();
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void DetectTagChange()
    {
        if (gameObject.tag != lastKnownTag)
        {
            lastKnownTag = gameObject.tag;

            if (queueManager != null)
            {
                queueManager.OnCustomerTagChanged(this, lastKnownTag);
            }
        }
    }

    private void CheckIfReachedExit()
    {
        if (!goingToExit) return;

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= arriveDistance)
        {
            if (queueManager != null)
            {
                queueManager.NotifyCustomerReachedExit(this);
            }
        }
    }

    public void SetTargetPosition(Vector3 newTarget)
    {
        goingToExit = false;
        targetPosition = newTarget;
    }

    public void GoToExit(Vector3 exitPosition)
    {
        goingToExit = true;
        targetPosition = exitPosition;
    }
}