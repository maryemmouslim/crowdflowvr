using UnityEngine;

public class VROverlay : MonoBehaviour
{
    public GameObject dangerOverlayPrefab;
    private GameObject overlay;
    private Renderer rend;

    // MLClient sera connecté plus tard par Maroua
    public bool dangerZoneDetected = false;

    void Start()
    {
        overlay = Instantiate(dangerOverlayPrefab, transform);
        rend = overlay.GetComponent<Renderer>();
        overlay.SetActive(false);
    }

    void Update()
    {
        overlay.SetActive(dangerZoneDetected);
        if (dangerZoneDetected)
        {
            float a = 0.25f + 0.15f * Mathf.Sin(Time.time * 5f);
            Color c = rend.material.color; c.a = a;
            rend.material.color = c;
        }
    }
}