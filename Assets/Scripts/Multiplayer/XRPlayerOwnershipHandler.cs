using System.Collections;
using UnityEngine;
using Unity.Netcode;

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
        // Don't trust ownership here anymore — wait for OnGainedOwnership
        if (!IsOwner)
        {
            Debug.Log("[XRPlayerOwnershipHandler] Not the owner. Disabling remote rig.");
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

        StartCoroutine(ReenableXRRigNextFrame());
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

    private IEnumerator ReenableXRRigNextFrame()
    {
        yield return null; 
        yield return null;

        Debug.Log("[XRPlayerOwnershipHandler] Forcing reactivation of XR rig to ensure tracking is reconnected.");


        if (xrOrigin.activeInHierarchy)
        {
            xrOrigin.SetActive(false);
            yield return null;
            xrOrigin.SetActive(true);
        }

    }

}
