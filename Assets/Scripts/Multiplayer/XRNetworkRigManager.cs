using System.Collections.Generic;
using UnityEngine;

public class XRNetworkRigManager : MonoBehaviour
{
    public static XRNetworkRigManager Instance;

    public List<GameObject> allRigs = new List<GameObject>();
    public float refreshInterval = 1f;

    private float refreshTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        refreshTimer += Time.deltaTime;
        if (refreshTimer >= refreshInterval)
        {
            RefreshRigList();
            StartCoroutine(DisableAndEnableAllRigs());
            refreshTimer = 0f;
        }
    }

    private void RefreshRigList()
    {
        allRigs.Clear();
        GameObject[] rigs = GameObject.FindGameObjectsWithTag("Player");
        allRigs.AddRange(rigs);
        Debug.Log($"[XRNetworkRigManager] Found {allRigs.Count} XR rigs.");
    }

    private System.Collections.IEnumerator DisableAndEnableAllRigs()
    {
        foreach (GameObject rig in allRigs)
        {
            if (rig != null)
                rig.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f); // Delay between disable and enable

        foreach (GameObject rig in allRigs)
        {
            if (rig != null)
                rig.SetActive(true);
        }

        Debug.Log("[XRNetworkRigManager] Refreshed all rigs.");
    }
}
