using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRNetworkRigManager : MonoBehaviour
{
    public static XRNetworkRigManager Instance;

    private List<GameObject> rigs = new List<GameObject>();
    private Coroutine refreshRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional if this should persist across scenes
    }

    public void RegisterRig(GameObject rig)
    {
        if (!rigs.Contains(rig))
        {
            Debug.Log($"[XRNetworkRigManager] Registered rig: {rig.name}");
            rigs.Add(rig);
        }

        // Always refresh on new rig registration
        if (refreshRoutine != null) StopCoroutine(refreshRoutine);
        refreshRoutine = StartCoroutine(RefreshAllRigs());
    }

    private IEnumerator RefreshAllRigs()
    {
        Debug.Log("[XRNetworkRigManager] Refreshing all rigs...");

        // Step 1: Disable all rigs
        foreach (var rig in rigs)
        {
            if (rig != null) rig.SetActive(false);
        }

        // Step 2: Wait for 0.5 seconds
        yield return new WaitForSeconds(4f);

        // Step 3: Enable all rigs
        foreach (var rig in rigs)
        {
            if (rig != null) rig.SetActive(true);
        }

        Debug.Log("[XRNetworkRigManager] Rigs re-enabled after delay.");
    }
}
