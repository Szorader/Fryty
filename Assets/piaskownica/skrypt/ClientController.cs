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

    public void SetClient(Client client)
    {
        clientData = client;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
}