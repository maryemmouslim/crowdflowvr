using UnityEngine;
using UnityEngine.AI;

public class CrowdAgent : MonoBehaviour
{
    public enum AgentState { Normal, Dense, Panic }
    public AgentState State = AgentState.Normal;

    public Transform[] exitPoints;
    public float normalSpeed = 1.4f;
    public float panicSpeed = 3.5f;

    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = normalSpeed;
        PickDestination();
    }

    void Update()
    {
        if (!nav.pathPending && nav.remainingDistance < 0.5f)
            PickDestination();
    }

    public void SetPanic(bool panic)
    {
        State = panic ? AgentState.Panic : AgentState.Normal;
        nav.speed = panic ? panicSpeed : normalSpeed;
    }

    public float GetSpeed()
    {
        return nav != null ? nav.velocity.magnitude : 0f;
    }

    void PickDestination()
    {
        if (exitPoints == null || exitPoints.Length == 0) return;
        Transform target = exitPoints[Random.Range(0, exitPoints.Length)];
        nav.SetDestination(target.position);
    }
}