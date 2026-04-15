using UnityEngine;
using UnityEngine.AI;

public class ClientController : MonoBehaviour
{
    //bad client and skin
    public ClientData clientData;
    
    public CustomerOrder customerOrder;
    public CustomerWaitingTime waitingTime;
    public CustomerSatisfaction satisfaction;
    
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        customerOrder = GetComponent<CustomerOrder>();
        waitingTime = GetComponent<CustomerWaitingTime>();
        satisfaction = GetComponent<CustomerSatisfaction>();
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

    public void SetClient(ClientData client)
    {
        clientData = client;

        if (clientData.isBadClient)
        {
            
            // bad client mechanics
        }
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        
    }
}