using Unity.Netcode;
using UnityEngine;

public class NetworkSync : NetworkBehaviour
{
    public static NetworkSync Instance;

    public NetworkVariable<bool> GlobalDanger =
        new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

    void Awake() => Instance = this;

    [ServerRpc(RequireOwnership = false)]
    public void SetDangerServerRpc(bool danger)
    {
        GlobalDanger.Value = danger;
    }

    [ClientRpc]
    public void TriggerEvacuationClientRpc()
    {
        Debug.Log("[NET] Évacuation déclenchée sur ce client");
    }

    public override void OnNetworkSpawn()
    {
        GlobalDanger.OnValueChanged += OnDangerChanged;
    }

    void OnDangerChanged(bool prev, bool next)
    {
        if (next) TriggerEvacuationClientRpc();
    }
}