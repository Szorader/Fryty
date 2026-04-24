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

    public GameObject eInteract;
    public GameObject orderText;
    
    public bool isBadClient = false;
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        customerOrder = GetComponent<CustomerOrder>();
        waitingTime = GetComponent<CustomerWaitingTime>();
        satisfaction = GetComponent<CustomerSatisfaction>();
        eInteract.SetActive(true);
        orderText.SetActive(false);
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

    public void Toggle()
    {
        eInteract.SetActive(false);
        orderText.SetActive(true);
    }
    
    void RotateTowardsZ()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void SetClient(ClientData client, string name, bool isBad)
    {
        clientData = client;
        customerOrder.clientName = name;
        Debug.Log("Client name: " + name);

        isBadClient = isBad;
        
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        
    }
}