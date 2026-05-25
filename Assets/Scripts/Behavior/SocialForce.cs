// Assets/Scripts/Behavior/SocialForce.cs
using UnityEngine;
using UnityEngine.AI;

public class SocialForce : MonoBehaviour
{
    [Header("Parametres Helbing (1995)")]
    public float A = 2000f;
    public float B = 0.08f;
    
    private NavMeshAgent nav;
    
    void Start() => nav = GetComponent<NavMeshAgent>();
    
    void FixedUpdate() {
        if (nav == null) return;

        Vector3 force = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, 3f);
        
        foreach (var col in neighbors) {
            if (col.gameObject == gameObject) continue;
            
            // On vérifie si le voisin a aussi un NavMeshAgent (c'est donc un autre agent)
            NavMeshAgent otherNav = col.GetComponent<NavMeshAgent>();
            if (otherNav == null) continue;
            
            Vector3 diff = transform.position - col.transform.position;
            float dist = Mathf.Max(diff.magnitude, 0.01f);
            force += (A * Mathf.Exp(-dist / B)) * diff.normalized;
        }
        
        nav.velocity += force * Time.fixedDeltaTime * 0.001f;
        if (nav.velocity.magnitude > nav.speed)
            nav.velocity = nav.velocity.normalized * nav.speed;
    }
}