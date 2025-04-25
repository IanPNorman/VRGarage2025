using Unity.Netcode;
using UnityEngine;

public class XRPlayerRig : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return; // Only have the local player do this

        Debug.Log($"[XRPlayerRig] Registering XR Origin with NetworkManager (clientId: {OwnerClientId})");

        
    }
}
