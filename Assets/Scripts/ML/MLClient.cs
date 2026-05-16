// Assets/Scripts/ML/MLClient.cs
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class MLClient : MonoBehaviour
{
    [Header("Serveur Python")]
    public string serverIP = "127.0.0.1";
    public int serverPort = 5005;
    public float interval = 0.2f;

    [HideInInspector] public bool dangerZoneDetected = false;
    [HideInInspector] public float predictedDensity = 0f;

    private UdpClient udp;

    void Start() 
    { 
        udp = new UdpClient(); 
        StartCoroutine(SendLoop()); 
    }

    IEnumerator SendLoop()
    {
        while (true)
        {
            try { SendAndReceive(); }
            catch (System.Exception e) 
            { Debug.LogWarning("ML: " + e.Message); }
            yield return new WaitForSeconds(interval);
        }
    }

    void SendAndReceive()
    {
        // Pour l'instant on envoie des données de test
        // Imane connectera les vrais agents plus tard
        var sb = new StringBuilder("{\"agents\":[");
        sb.Append("{\"x\":0,\"z\":0,\"spd\":1.4}");
        sb.Append("]}");

        byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
        udp.Send(data, data.Length, serverIP, serverPort);
    }

    void OnDestroy() => udp?.Close();
}