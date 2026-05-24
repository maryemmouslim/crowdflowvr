using UnityEngine;
using UnityEngine.AI;

public class PietonController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] destinations;
    private int index = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[index].position);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            index = (index + 1) % destinations.Length;
            agent.SetDestination(destinations[index].position);
        }
    }
}