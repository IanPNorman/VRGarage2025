using UnityEngine;
using Unity.Netcode;

public class RoleActivator : NetworkBehaviour
{
    [Header("Game Master Scripts")]
    public MonoBehaviour[] gameMasterScripts;

    [Header("Survivor Scripts")]
    public MonoBehaviour[] survivorScripts;

    private NetworkedPlayer networkedPlayer;

    private void Start()
    {
        if (!IsOwner) return; // Only run on the local player

        networkedPlayer = GetComponent<NetworkedPlayer>();
        if (networkedPlayer == null)
        {
            Debug.LogError("RoleBasedScriptManager: NetworkedPlayer reference missing.");
            return;
        }

        ApplyRole(networkedPlayer.playerRole.Value);

        // Listen for runtime changes (in case role is set slightly after spawn)
        networkedPlayer.playerRole.OnValueChanged += (oldVal, newVal) => ApplyRole(newVal);
    }

    private void ApplyRole(PlayerRole role)
    {
        Debug.Log($"Applying role-based logic: {role}");

        bool isGM = role == PlayerRole.GameMaster;

        foreach (var script in gameMasterScripts)
            script.enabled = isGM;

        foreach (var script in survivorScripts)
            script.enabled = !isGM;
    }
}
