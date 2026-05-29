using UnityEngine;


public class BarrierPlacer : MonoBehaviour
{
    public GameObject barrierPrefab;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor ray;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) TryPlaceBarrier();
    }

    public void TryPlaceBarrier()
    {
        if (ray != null && ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 pos = hit.point;
            pos.y = 0.75f;
            Instantiate(barrierPrefab, pos, Quaternion.identity);
        }
    }
}