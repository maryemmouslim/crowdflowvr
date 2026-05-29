using UnityEngine;
using System.Collections;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform[] spawnPoints;
    public Transform[] exitPoints;
    public int maxAgents = 50;
    public float spawnRate = 1f;

    private int activeAgents = 0;
    private MLClient mlClient;

    void Start()
    {
        mlClient = FindObjectOfType<MLClient>();
        StartCoroutine(SpawnLoop());
        StartCoroutine(CheckDanger());
    }

    IEnumerator SpawnLoop()
    {
        while (activeAgents < maxAgents)
        {
            SpawnAgent();
            yield return new WaitForSeconds(1f / spawnRate);
        }
    }

    void SpawnAgent()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = sp.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        GameObject go = Instantiate(agentPrefab, pos, Quaternion.identity);
        CrowdAgent agent = go.GetComponent<CrowdAgent>();
        agent.exitPoints = exitPoints;
        activeAgents++;
    }

    IEnumerator CheckDanger()
    {
        while (true)
        {
            if (mlClient != null && mlClient.dangerDetected)
            {
                // Mettre tous les agents en panique
                CrowdAgent[] agents = FindObjectsOfType<CrowdAgent>();
                foreach (var a in agents)
                    a.SetPanic(true);
                Debug.Log("DANGER detecte par ML - agents en panique !");
            }
            else
            {
                CrowdAgent[] agents = FindObjectsOfType<CrowdAgent>();
                foreach (var a in agents)
                    a.SetPanic(false);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
