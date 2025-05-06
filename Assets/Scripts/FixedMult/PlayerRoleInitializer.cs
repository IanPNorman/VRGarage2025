using Unity.Netcode;
using UnityEngine;

public class PlayerRoleInitializer : NetworkBehaviour
{
    public NetworkVariable<PlayerRole> NetworkRole = new NetworkVariable<PlayerRole>(
        PlayerRole.None,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    [Header("GameMaster Objects")]
    public GameObject[] gameMasterObjects;

    [Header("Survivor Objects")]
    public GameObject[] survivorObjects;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        NetworkRole.OnValueChanged += OnRoleChanged;

        // Initialize role if already assigned
        SetupRoleObjects(NetworkRole.Value);
    }

    private void OnRoleChanged(PlayerRole oldRole, PlayerRole newRole)
    {
        SetupRoleObjects(newRole);
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

    private void OnDestroy()
    {
        if (IsOwner)
        {
            NetworkRole.OnValueChanged -= OnRoleChanged;
        }
    }
}
