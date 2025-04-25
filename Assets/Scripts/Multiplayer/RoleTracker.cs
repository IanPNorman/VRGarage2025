using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleManager : NetworkBehaviour
{
    public static RoleManager Instance;

    public NetworkVariable<int> gameMasterCount = new NetworkVariable<int>(0);
    public NetworkVariable<int> survivorCount = new NetworkVariable<int>(0);

    private Dictionary<ulong, PlayerRole> playerRoles = new Dictionary<ulong, PlayerRole>();

    public const int MaxGameMasters = 1;
    public const int MaxSurvivors = 4;

    public bool AllRolesAssigned => gameMasterCount.Value == 1 && survivorCount.Value >= 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Avoid duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;

        // Optional: Only the server should manage this object
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestRoleServerRpc(ulong clientId, PlayerRole requestedRole)
    {
        if (playerRoles.ContainsKey(clientId)) return;

        if (requestedRole == PlayerRole.GameMaster && gameMasterCount.Value < MaxGameMasters)
        {
            playerRoles[clientId] = PlayerRole.GameMaster;
            gameMasterCount.Value++;
        }
        else if (requestedRole == PlayerRole.Survivor && survivorCount.Value < MaxSurvivors)
        {
            playerRoles[clientId] = PlayerRole.Survivor;
            survivorCount.Value++;
        }
    }

    public PlayerRole GetRole(ulong clientId)
    {
        return playerRoles.TryGetValue(clientId, out var role) ? role : PlayerRole.None;
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        Debug.Log("Start Game triggered");

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (!playerRoles.ContainsKey(client.ClientId))
            {
                if (survivorCount.Value < MaxSurvivors)
                {
                    playerRoles[client.ClientId] = PlayerRole.Survivor;
                    survivorCount.Value++;
                }
            }
        }

        NetworkManager.Singleton.SceneManager.LoadScene("DemoMap", LoadSceneMode.Single);
    }
}

public enum PlayerRole
{
    None,
    GameMaster,
    Survivor
}
