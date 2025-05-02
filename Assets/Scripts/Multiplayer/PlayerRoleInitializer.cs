using Unity.Netcode;
using UnityEngine;

public class PlayerRoleInitializer : NetworkBehaviour
{
    [Header("GameMaster Objects")]
    public GameObject[] gameMasterObjects;

    [Header("Survivor Objects")]
    public GameObject[] survivorObjects;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        // Get this client's ID
        ulong clientId = NetworkManager.Singleton.LocalClientId;

        // Ask the RoleManager for this player's role
        PlayerRole role = RoleManager.Instance.GetRole(clientId);

        // Set up based on role
        SetupRoleObjects(role);
    }

    private void SetupRoleObjects(PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.GameMaster:
                SetActiveArray(gameMasterObjects, true);
                SetActiveArray(survivorObjects, false);
                break;

            case PlayerRole.Survivor:
                SetActiveArray(gameMasterObjects, false);
                SetActiveArray(survivorObjects, true);
                break;

            default:
                SetActiveArray(gameMasterObjects, false);
                SetActiveArray(survivorObjects, false);
                break;
        }
    }

    private void SetActiveArray(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
