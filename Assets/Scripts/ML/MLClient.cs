using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class MLClient : MonoBehaviour
{
    public string mlServerIP = "127.0.0.1";
    public int mlServerPort = 5005;
    public float sendInterval = 0.5f;

    [HideInInspector] public bool dangerDetected = false;
    [HideInInspector] public float density = 0f;

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
            try { SendData(); }
            catch (System.Exception e) { Debug.LogWarning("ML: " + e.Message); }
            yield return new WaitForSeconds(sendInterval);
        }
    }

    void SendData()
    {
        MonoBehaviour[] agents = FindObjectsOfType<PietonController>();
        var sb = new StringBuilder();
        sb.Append("{\"agents\":[");
        bool first = true;
        foreach (var a in agents)
        {
            UnityEngine.AI.NavMeshAgent nav = a.GetComponent<UnityEngine.AI.NavMeshAgent>();
            float spd = nav != null ? nav.velocity.magnitude : 0f;
            if (!first) sb.Append(",");
            sb.Append($"{{\"x\":{a.transform.position.x:F2},\"z\":{a.transform.position.z:F2},\"spd\":{spd:F2}}}");
            first = false;
        }
        sb.Append("]}");

        byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
        udp.Send(data, data.Length, mlServerIP, mlServerPort);

        udp.Client.ReceiveTimeout = 200;
        try
        {
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
            byte[] response = udp.Receive(ref ep);
            var json = Encoding.UTF8.GetString(response);
            var result = JsonUtility.FromJson<MLResponse>(json);
            dangerDetected = result.danger;
            density = result.density;
        }
        catch { }
    }

    void OnDestroy() { udp?.Close(); }
}

[System.Serializable]
public class MLResponse
{
    public bool danger;
    public float density;
    public float score;
}