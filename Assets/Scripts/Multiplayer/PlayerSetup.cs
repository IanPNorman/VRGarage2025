using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return; // Only modify the local player’s prefab

        // Delay a bit to ensure RoleManager is ready
        StartCoroutine(SetupPlayerAfterDelay());
    }

    private System.Collections.IEnumerator SetupPlayerAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Give RoleManager time to initialize if needed

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        PlayerRole role = RoleManager.Instance.GetRole(clientId);

        SetupBasedOnRole(role);
    }

    private void SetupBasedOnRole(PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.GameMaster:
                transform.localScale = Vector3.one * 6f;

                var xrOrigin = GetComponentInChildren<XROrigin>();
                if (xrOrigin != null)
                {
                    xrOrigin.gameObject.name = "GameMaster XR Origin";
                }

                gameObject.tag = "gameMaster"; // Make sure the tag exists in the Tag Manager!
                break;

            case PlayerRole.Survivor:
                // Optional: do Survivor setup
                break;

            default:
                Debug.LogWarning("Player role not set or unknown.");
                break;
        }
    }
}
