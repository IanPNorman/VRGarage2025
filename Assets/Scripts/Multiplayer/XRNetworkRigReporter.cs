using Unity.Netcode;
using UnityEngine;

public class XRNetworkRigReporter : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("[XRNetworkRigReporter] I'm the owner of this rig. Requesting all clients to register it.");
            RegisterRigClientRpc(NetworkObjectId);
        }
    }

    [ClientRpc]
    private void RegisterRigClientRpc(ulong rigId)
    {
        NetworkObject obj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[rigId];
        GameObject rigGO = obj.gameObject;

        if (XRNetworkRigManager.Instance != null)
        {
            XRNetworkRigManager.Instance.RegisterRig(rigGO);
        }
        else
        {
            Debug.LogWarning("[XRNetworkRigReporter] XRNetworkRigManager not found.");
        }
    }
}
