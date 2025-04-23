using Unity.Netcode;
using UnityEngine;

public class LobbyPlayerSpawner : NetworkBehaviour
{
    public GameObject lobbySurvivorPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject != null)
        {
            Debug.Log($"[LobbyPlayerSpawner] Client {OwnerClientId} already has a PlayerObject. Skipping spawn.");
            return;
        }

        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        GameObject instance = Instantiate(lobbySurvivorPrefab, spawnPos, Quaternion.identity);
        instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId, true);

        Debug.Log($"[LobbyPlayerSpawner] Spawned lobby prefab for client {OwnerClientId}");
    }
}
