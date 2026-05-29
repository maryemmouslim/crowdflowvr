using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CrowdSpawner : MonoBehaviour
{
    [Header("Prefab et points de navigation")]
    public GameObject agentPrefab;
    public Transform[] spawnPoints;
    public Transform[] exitPoints;
    public Transform emergencyExit;

    [Header("Generation")]
    public int maxAgents = 10;
    public float spawnRate = 1f;

    [Header("Test local uniquement")]
    public Key evacuationTestKey = Key.E;

    private readonly List<CrowdAgent> agents = new List<CrowdAgent>();
    private MLClient mlClient;
    private bool evacuationTriggered = false;
    private bool lastDangerState = false;

    private void Start()
    {
        mlClient = FindObjectOfType<MLClient>();

        StartCoroutine(SpawnLoop());
        StartCoroutine(CheckDanger());
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current[evacuationTestKey].wasPressedThisFrame)
        {
            TriggerEvacuation();
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (agents.Count < maxAgents)
        {
            SpawnAgent();

            float safeRate = Mathf.Max(0.1f, spawnRate);
            yield return new WaitForSeconds(1f / safeRate);
        }
    }

    private void SpawnAgent()
    {
        if (agentPrefab == null)
        {
            Debug.LogError("CrowdSpawner : Agent Prefab non renseigne.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("CrowdSpawner : aucun Spawn Point renseigne.");
            return;
        }

        Transform selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (selectedSpawn == null)
            return;

        Vector3 requestedPosition = selectedSpawn.position +
            new Vector3(Random.Range(-0.6f, 0.6f), 0f, Random.Range(-0.6f, 0.6f));

        if (!NavMesh.SamplePosition(requestedPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            Debug.LogWarning("CrowdSpawner : aucun NavMesh proche du point de generation.");
            return;
        }

        GameObject createdObject = Instantiate(agentPrefab, hit.position, Quaternion.identity);
        CrowdAgent agent = createdObject.GetComponent<CrowdAgent>();

        if (agent == null)
        {
            Debug.LogError("CrowdSpawner : le prefab ne contient pas CrowdAgent.");
            Destroy(createdObject);
            return;
        }

        agent.exitPoints = exitPoints;
        agent.emergencyExit = emergencyExit;

        if (evacuationTriggered)
            agent.SetState(CrowdAgent.AgentState.Evacuation);
        else
            agent.SetState(CrowdAgent.AgentState.Normal);

        agents.Add(agent);
    }

    private IEnumerator CheckDanger()
    {
        while (true)
        {
            if (!evacuationTriggered)
            {
                bool danger = mlClient != null && mlClient.dangerDetected;

                foreach (CrowdAgent agent in agents)
                {
                    if (agent != null)
                        agent.SetPanic(danger);
                }

                if (danger && !lastDangerState)
                    Debug.Log("DANGER detecte par ML : agents en panique.");

                lastDangerState = danger;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void TriggerEvacuation()
    {
        evacuationTriggered = true;

        foreach (CrowdAgent agent in agents)
        {
            if (agent != null)
                agent.SetState(CrowdAgent.AgentState.Evacuation);
        }

        Debug.Log("EVACUATION declenchee : tous les agents vont vers la sortie d'urgence.");
    }
}