using UnityEngine;
using Unity.Netcode;

public class NetworkedPlayer : NetworkBehaviour
{
    public NetworkVariable<PlayerRole> playerRole = new NetworkVariable<PlayerRole>(
        PlayerRole.None,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Look up the role from RoleManager and assign it to this player
            var assignedRole = RoleManager.Instance.GetRole(OwnerClientId);
            playerRole.Value = assignedRole;
        }
    }
}
