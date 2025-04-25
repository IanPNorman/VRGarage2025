using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XRRefreshManager : MonoBehaviour
{
    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[XRRefreshManager] New client connected: {clientId}. Refreshing all XR Rigs.");
        StartCoroutine(RefreshAllRigs());
    }

    private IEnumerator RefreshAllRigs()
    {
        // Wait for player rig to be instantiated
        yield return new WaitForSeconds(0.5f);

        GameObject[] rigs = GameObject.FindGameObjectsWithTag("LobbyRig");

        Debug.Log($"[XRRefreshManager] Found {rigs.Length} rigs to refresh.");

        foreach (GameObject rig in rigs)
        {
            if (rig != null) rig.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f); // Delay between disable and enable

        foreach (GameObject rig in rigs)
        {
            if (rig != null) rig.SetActive(true);
        }

        Debug.Log("[XRRefreshManager] XR rigs re-enabled.");
    }
}
