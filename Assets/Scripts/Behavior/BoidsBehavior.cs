// Assets/Scripts/Behavior/BoidsBehavior.cs
using UnityEngine;
using UnityEngine.AI;

public class BoidsBehavior : MonoBehaviour
{
    [Header("Parametres de Separation Boids")]
    public float separationRadius = 1.5f;
    public float separationStrength = 5f;

    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (nav == null) return;

        Vector3 separationForce = Vector3.zero;
        int neighborCount = 0;

        // Trouver les agents à proximité
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius);

        foreach (var col in neighbors)
        {
            if (col.gameObject == gameObject) continue;
            
            // Correction ici : on cherche si le voisin possède un NavMeshAgent (c'est donc un agent)
            NavMeshAgent otherNav = col.GetComponent<NavMeshAgent>();
            if (otherNav == null) continue;

            neighborCount++;
            Vector3 diff = transform.position - col.transform.position;
            
            // Plus ils sont proches, plus la force de répulsion est forte
            float distance = diff.magnitude;
            if (distance > 0f)
            {
                separationForce += diff.normalized / distance;
            }
        }

        if (neighborCount > 0)
        {
            // Appliquer la force accumulée à la vitesse de l'agent
            nav.velocity += separationForce * separationStrength * Time.fixedDeltaTime;
        }
    }
}