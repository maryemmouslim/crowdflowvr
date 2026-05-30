using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class DesktopController : MonoBehaviour
{
    [Header("Caméra top-down")]
    public float height = 22f;
    public float moveSpeed = 20f;

    Camera cam;

    void Start()
    {
        if (!UnityEngine.XR.XRSettings.isDeviceActive)
        {
            cam = GetComponent<Camera>();
            cam.transform.position = new Vector3(0, height, 0);
            cam.transform.rotation = Quaternion.Euler(90, 0, 0);
            Debug.Log("[DESKTOP] Mode PC activé — VR absent");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 move = new Vector2(
            Keyboard.current != null && Keyboard.current.dKey.isPressed ? 1 :
            Keyboard.current != null && Keyboard.current.aKey.isPressed ? -1 : 0,
            Keyboard.current != null && Keyboard.current.wKey.isPressed ? 1 :
            Keyboard.current != null && Keyboard.current.sKey.isPressed ? -1 : 0
        );

        transform.Translate(new Vector3(move.x, 0, move.y) * moveSpeed * Time.deltaTime, Space.World);

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (NetworkSync.Instance != null)
                NetworkSync.Instance.SetDangerServerRpc(true);
            Debug.Log("[DESKTOP] Touche E → évacuation envoyée");
        }
    }
}