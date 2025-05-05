using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : NetworkBehaviour
{
    public static RoleManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public NetworkVariable<int> gameMasterCount = new NetworkVariable<int>(0);
    public NetworkVariable<int> survivorCount = new NetworkVariable<int>(0);

    private Dictionary<ulong, PlayerRole> playerRoles = new Dictionary<ulong, PlayerRole>();

    public const int MaxGameMasters = 1;
    public const int MaxSurvivors = 4;

    [ServerRpc(RequireOwnership = false)]
    public void RequestRoleServerRpc(ulong clientId, PlayerRole requestedRole, ServerRpcParams rpcParams = default)
    {
        if (playerRoles.ContainsKey(clientId)) return;

        if (requestedRole == PlayerRole.GameMaster && gameMasterCount.Value < MaxGameMasters)
        {
            playerRoles[clientId] = PlayerRole.GameMaster;
            gameMasterCount.Value++;
            Debug.Log($"Client {clientId} is now Game Master.");
            AssignRoleToPlayerObject(clientId, PlayerRole.GameMaster);
        }
        else if (requestedRole == PlayerRole.Survivor && survivorCount.Value < MaxSurvivors)
        {
            playerRoles[clientId] = PlayerRole.Survivor;
            survivorCount.Value++;
            Debug.Log($"Client {clientId} is now Survivor.");
            AssignRoleToPlayerObject(clientId, PlayerRole.Survivor);
        }
    }

    private void AssignRoleToPlayerObject(ulong clientId, PlayerRole role)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            var playerObject = client.PlayerObject;
            if (playerObject != null && playerObject.TryGetComponent(out PlayerRoleInitializer initializer))
            {
                initializer.NetworkRole.Value = role;
            }
        }
    }

    public PlayerRole GetRole(ulong clientId)
    {
        return playerRoles.TryGetValue(clientId, out var role) ? role : PlayerRole.None;
    }
}

public enum PlayerRole
{
    None,
    GameMaster,
    Survivor
}
