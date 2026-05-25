// Assets/Scripts/Behavior/PanicTrigger.cs
using UnityEngine;
using System.Collections;

public class PanicTrigger : MonoBehaviour
{
    public float panicRadius = 5f;
    public float propagDelay = 0.3f;
    public KeyCode testKey = KeyCode.P;

    void Update() 
    { 
        if (Input.GetKeyDown(testKey)) TriggerPanic(); 
    }

    public void TriggerPanic() 
    {
        Debug.Log("PANIQUE declenhee depuis " + transform.position);
        StartCoroutine(Propagate());
    }

    IEnumerator Propagate() 
    {
        float r = panicRadius;
        while (r < 35f) 
        {
            foreach (var col in Physics.OverlapSphere(transform.position, r)) 
            {
                // Modification générique : on cherche n'allant pas directement nommer la classe d'agent
                // et on applique un message ou un comportement générique pour le test
                if (col.CompareTag("Agent") || col.gameObject.name.Contains("Agent"))
                {
                    Debug.Log("Agent touche par la panique : " + col.gameObject.name);
                }
            }
            r += 3f;
            yield return new WaitForSeconds(propagDelay);
        }
    }

    void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, panicRadius); 
    }
}