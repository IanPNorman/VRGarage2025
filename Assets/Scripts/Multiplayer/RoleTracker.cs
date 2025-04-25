using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleManager : NetworkBehaviour
{
    public static RoleManager Instance;

    [Header("Role-Based Prefabs")]
    public GameObject gameMasterPrefab;
    public GameObject survivorPrefab;
    public GameObject lobbyPlayerPrefab;

    private Dictionary<ulong, PlayerRole> playerRoles = new Dictionary<ulong, PlayerRole>();

    public NetworkVariable<int> gameMasterCount = new NetworkVariable<int>(0);
    public NetworkVariable<int> survivorCount = new NetworkVariable<int>(0);

    public const int MaxGameMasters = 1;
    public const int MaxSurvivors = 4;

    public bool AllRolesAssigned => gameMasterCount.Value == 1 && survivorCount.Value >= 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;

            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                Debug.Log($"[RoleManager] Client {clientId} connected.");
                // ✅ Refresh their local XR rig
                XRNetworkRigManager.Instance?.TriggerRefresh(clientId);
            };
        }
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
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (!playerRoles.ContainsKey(client.ClientId) && survivorCount.Value < MaxSurvivors)
            {
                playerRoles[client.ClientId] = PlayerRole.Survivor;
                survivorCount.Value++;
            }
        }

        NetworkManager.Singleton.SceneManager.LoadScene("DemoMap", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        if (!IsServer) return;

        var client = NetworkManager.Singleton.ConnectedClients[clientId];

        if (client.PlayerObject != null)
        {
            Debug.Log($"[OnSceneLoaded] Client {clientId} already has PlayerObject. Skipping.");
            return;
        }

        GameObject prefabToSpawn;

        if (sceneName == "BasicScene")
        {
            prefabToSpawn = lobbyPlayerPrefab;
        }
        else
        {
            PlayerRole role = GetRole(clientId);
            prefabToSpawn = role == PlayerRole.GameMaster ? gameMasterPrefab : survivorPrefab;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError($"[OnSceneLoaded] No prefab found for scene: {sceneName}");
            return;
        }

        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        GameObject player = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        Debug.Log($"[OnSceneLoaded] Spawned {prefabToSpawn.name} for client {clientId}");
    }
}

public enum PlayerRole
{
    None,
    GameMaster,
    Survivor
}
