using UnityEngine;

public class VROverlay : MonoBehaviour
{
    public GameObject dangerOverlayPrefab;
    public MLClient mlClient;

    private GameObject overlay;
    private Renderer rend;

    void Start()
    {
        overlay = Instantiate(dangerOverlayPrefab, transform);
        rend = overlay.GetComponent<Renderer>();
        overlay.SetActive(false);
    }

    void Update()
    {
        if (mlClient == null)
            return;

        bool danger = mlClient.dangerDetected;

        overlay.SetActive(danger);

        if (danger)
        {
            float a = 0.25f + 0.15f * Mathf.Sin(Time.time * 5f);

            Color c = rend.material.color;
            c.a = a;

            rend.material.color = c;
        }
    }
}