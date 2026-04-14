using UnityEngine;
using UnityEngine.AI;

public class ClientController : MonoBehaviour
{
    public Client clientData;
    
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                RotateTowardsZ();
            }
        }
    }
    
    void RotateTowardsZ()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void SetClient(Client client)
    {
        clientData = client;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        
    }
}