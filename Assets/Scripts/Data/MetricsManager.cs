using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricsManager : MonoBehaviour
{
    [Header("Configuration Densité")]
    [SerializeField] private float detectionRadius = 3f; // Rayon de recherche autour d'un agent (en mètres)
    [SerializeField] private int criticalDensityThreshold = 8; // Nb d'agents max tolérés dans ce rayon

    [Header("Statistiques Temps Réel")]
    public int activeAgentsCount = 0;
    public int evacuatedAgentsCount = 0;
    public string currentAlertZone = "Aucune";

    void Update()
    {
        MonitorCrowdDensity();
    }

    /// <summary>
    /// Calcule la densité de la foule et détecte les zones de danger
    /// </summary>
    void MonitorCrowdDensity()
    {
        CrowdAgent[] agents = FindObjectsOfType<CrowdAgent>();
        activeAgentsCount = agents.Length;

        if (activeAgentsCount == 0)
        {
            currentAlertZone = "Aucune (Scène vide)";
            return;
        }

        int highestLocalDensity = 0;
        Vector3 dangerZonePosition = Vector3.zero;
        bool alertTriggered = false;

        // Algorithme de proximité (N² optimisé par la taille de la foule)
        for (int i = 0; i < agents.Length; i++)
        {
            int localCount = 0;
            Vector3 agentPos = agents[i].transform.position;

            for (int j = 0; j < agents.Length; j++)
            {
                // On calcule la distance entre l'agent i et l'agent j
                if (Vector3.Distance(agentPos, agents[j].transform.position) <= detectionRadius)
                {
                    localCount++;
                }
            }

            // On garde en mémoire la zone où le regroupement est le plus critique
            if (localCount > highestLocalDensity)
            {
                highestLocalDensity = localCount;
                dangerZonePosition = agentPos;
            }
        }

        // Déclenchement de l'alerte si le seuil critique est dépassé
        if (highestLocalDensity >= criticalDensityThreshold)
        {
            alertTriggered = true;
            currentAlertZone = $"⚠️ Danger : Regroupement de {highestLocalDensity} agents à la position ({dangerZonePosition.x:F1}, {dangerZonePosition.z:F1})";

            // Log visuel dans la console Unity
            Debug.LogWarning($"[ALERT METRICS] {currentAlertZone}");
        }

        if (!alertTriggered)
        {
            currentAlertZone = "Normale (Foule fluide)";
        }
    }

    /// <summary>
    /// Méthode publique à appeler par le script de la zone de sortie
    /// </summary>
    public void IncrementEvacuatedAgents()
    {
        evacuatedAgentsCount++;
        Debug.Log($"<color=green>[SUCCESS]</color> Un agent a été évacué. Total : {evacuatedAgentsCount}");
    }
}