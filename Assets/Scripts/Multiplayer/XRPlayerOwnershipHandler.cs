using Unity.Netcode;
using UnityEngine;

public class XRPlayerOwnershipHandler : NetworkBehaviour
{
    [Header("Local Player Only Components")]
    [SerializeField] private GameObject xrOrigin;                  // Your XR rig root (camera + controllers)
    [SerializeField] private MonoBehaviour[] inputScripts;         // Movement, interaction, etc.
    [SerializeField] private Camera[] camerasToDisable;            // In case you have extra cameras

    [Header("Remote Visuals (optional)")]
    [SerializeField] private GameObject remoteAvatar;              // Head/hands for remote players

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            EnableLocalRig();
        }
        else
        {
            Debug.Log("[XRPlayerOwnershipHandler] Remote player detected. Disabling XR Rig.");
            DisableRemoteRig();
        }
    }

    private void EnableLocalRig()
    {
        if (xrOrigin != null) xrOrigin.SetActive(true);
        if (remoteAvatar != null) remoteAvatar.SetActive(false);

        foreach (var script in inputScripts)
        {
            if (script != null)
                script.enabled = true;
        }

        foreach (var cam in camerasToDisable)
        {
            if (cam != null)
                cam.enabled = true;
        }
    }

    private void DisableRemoteRig()
    {
        if (xrOrigin != null) xrOrigin.SetActive(false);
        if (remoteAvatar != null) remoteAvatar.SetActive(true);

        foreach (var script in inputScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        foreach (var cam in camerasToDisable)
        {
            if (cam != null)
                cam.enabled = false;
        }
    }
}
