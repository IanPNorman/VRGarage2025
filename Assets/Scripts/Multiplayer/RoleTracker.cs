using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleManager : NetworkBehaviour
{
    public static RoleManager Instance;

    [Header("Player Prefabs")]
    public GameObject lobbySurvivorPrefab; // 👈 NEW: Used only in the Lobby scene
    public GameObject gameMasterPrefab;
    public GameObject survivorPrefab;

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
        DontDestroyOnLoad(gameObject); // ✅ Keeps RoleManager alive between scenes
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;
            Debug.Log("[RoleManager] Hooked OnSceneLoaded via OnNetworkSpawn");
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

        foreach (var kvp in playerRoles)
        {
            Debug.Log($"[BEFORE LOAD] Client {kvp.Key} = {kvp.Value}");
        }

        NetworkManager.Singleton.SceneManager.LoadScene("DemoMap", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!IsServer) return;

        var client = NetworkManager.Singleton.ConnectedClients[clientId];

        // 🧨 Destroy any existing PlayerObject before spawning a new one
        if (client.PlayerObject != null)
        {
            Debug.Log($"[OnSceneLoaded] Client {clientId} already has a PlayerObject. Destroying it to spawn a new one.");
            client.PlayerObject.Despawn(true); // Despawn and destroy
        }

        // Choose prefab based on role
        PlayerRole role = GetRole(clientId);
        Debug.Log($"[OnSceneLoaded] Game scene: Role for Client {clientId} is {role}");

        GameObject prefabToSpawn = role == PlayerRole.GameMaster ? gameMasterPrefab : survivorPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError($"[OnSceneLoaded] Prefab for role {role} is NULL! Check your RoleManager inspector.");
            return;
        }

        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        GameObject instance = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        Debug.Log($"[OnSceneLoaded] Spawned {prefabToSpawn.name} for client {clientId}");
    }
}

    public enum PlayerRole
{
    None,
    GameMaster,
    Survivor
}
