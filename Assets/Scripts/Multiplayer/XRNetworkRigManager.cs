using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XRNetworkRigManager : NetworkBehaviour
{
    public static XRNetworkRigManager Instance;

    [Header("Tracked XR Rigs")]
    public List<GameObject> xrRigs = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Only tell the specific client to refresh their XR rig
        TriggerRefresh(clientId);
    }

    public void TriggerRefresh(ulong clientId)
    {
        if (IsServer)
        {
            RefreshRigClientRpc(clientId);
        }
    }

    [ClientRpc]
    private void RefreshRigClientRpc(ulong targetClientId)
    {
        if (NetworkManager.Singleton.LocalClientId != targetClientId)
            return;

        Debug.Log($"[XRNetworkRigManager] Refreshing XR rig for local client {targetClientId}...");
        StartCoroutine(RefreshMyRig());
    }

    private IEnumerator RefreshMyRig()
    {
        GameObject myRig = null;

        var allRigs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var rig in allRigs)
        {
            if (rig.CompareTag("XRPlayer") && rig.scene.name != null)
            {
                var netObj = rig.GetComponent<NetworkObject>();
                if (netObj != null && netObj.IsOwner)
                {
                    myRig = rig;
                    break;
                }
            }
        }

        if (myRig == null)
        {
            Debug.LogWarning("[XRNetworkRigManager] Could not find local XR rig to refresh.");
            yield break;
        }

        myRig.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        myRig.SetActive(true);

        Debug.Log("[XRNetworkRigManager] XR rig successfully refreshed.");
    }
}
