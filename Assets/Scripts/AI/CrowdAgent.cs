using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CrowdAgent : MonoBehaviour
{
    public enum AgentState
    {
        Normal,
        Dense,
        Panic,
        Evacuation
    }

    [Header("Etat actuel")]
    public AgentState State = AgentState.Normal;

    [Header("Destinations")]
    public Transform[] exitPoints;
    public Transform emergencyExit;

    [Header("Vitesses")]
    public float normalSpeed = 1.4f;
    public float denseSpeed = 0.8f;
    public float panicSpeed = 3.5f;
    public float evacuationSpeed = 2.0f;

    private NavMeshAgent nav;

    public float Speed => nav != null ? nav.velocity.magnitude : 0f;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ApplySpeed();

        if (State == AgentState.Evacuation)
            GoToEmergencyExit();
        else
            PickDestination();
    }

    private void Update()
    {
        if (!IsReady())
            return;

        if (State == AgentState.Evacuation)
        {
            if (emergencyExit != null &&
                !nav.pathPending &&
                !nav.hasPath)
            {
                GoToEmergencyExit();
            }

            return;
        }

        if (!nav.pathPending &&
            (!nav.hasPath || nav.remainingDistance <= nav.stoppingDistance + 0.1f))
        {
            PickDestination();
        }
    }

    public void SetState(AgentState newState)
    {
        bool stateChanged = State != newState;
        State = newState;

        ApplySpeed();

        if (!IsReady())
            return;

        if (State == AgentState.Evacuation)
        {
            GoToEmergencyExit();
        }
        else if (stateChanged || !nav.hasPath)
        {
            PickDestination();
        }
    }

    public void SetPanic(bool panic)
    {
        if (State == AgentState.Evacuation)
            return;

        SetState(panic ? AgentState.Panic : AgentState.Normal);
    }

    public float GetSpeed()
    {
        return Speed;
    }

    private void ApplySpeed()
    {
        if (nav == null)
            nav = GetComponent<NavMeshAgent>();

        if (nav == null)
            return;

        switch (State)
        {
            case AgentState.Normal:
                nav.speed = normalSpeed;
                break;

            case AgentState.Dense:
                nav.speed = denseSpeed;
                break;

            case AgentState.Panic:
                nav.speed = panicSpeed;
                break;

            case AgentState.Evacuation:
                nav.speed = evacuationSpeed;
                break;
        }
    }

    private void PickDestination()
    {
        if (!IsReady() || exitPoints == null || exitPoints.Length == 0)
            return;

        Transform target = exitPoints[Random.Range(0, exitPoints.Length)];

        if (target != null)
            nav.SetDestination(target.position);
    }

    private void GoToEmergencyExit()
    {
        if (!IsReady())
            return;

        if (emergencyExit != null)
            nav.SetDestination(emergencyExit.position);
    }

    private bool IsReady()
    {
        return nav != null && nav.enabled && nav.isOnNavMesh;
    }
}