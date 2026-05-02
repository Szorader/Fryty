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
    
    private Animator animator;
    
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        customerOrder = GetComponent<CustomerOrder>();
        waitingTime = GetComponent<CustomerWaitingTime>();
        satisfaction = GetComponent<CustomerSatisfaction>();
        eInteract.SetActive(true);
        orderText.SetActive(false);
        animator = GetComponent<Animator>();
        agent.stoppingDistance = 0.05f;
    }
    
    
    void Update()
    {
        animator.SetBool("isWalking", agent.velocity.sqrMagnitude > 0.05f);
        //Debug.Log(agent.updateRotation);
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
                {
                    RotateTowardsZ();
                    //animator.SetBool("isWalking", false);
                }
            }
        }
    
    }
    
    //promnt nad glowa
    public void Toggle()
    {
        eInteract.SetActive(false);
        orderText.SetActive(true);
    }
    
    //obrot w strone trucka
    void RotateTowardsZ()
    {
        agent.updateRotation = false;
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
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.updateRotation = true;
            Debug.Log("move to: " + hit.position);
            agent.SetDestination(hit.position);
            //animator.SetBool("isWalking", true);
        }
        else
        {
            Debug.LogWarning("Point not on NavMesh!");
        }
        /*
        animator.SetBool("isWalking", true);
        agent.SetDestination(position);
        Debug.Log(position);
        */
    }
}