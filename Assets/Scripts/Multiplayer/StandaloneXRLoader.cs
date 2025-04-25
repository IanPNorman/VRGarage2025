using UnityEngine;
using UnityEngine.XR;

public class StandaloneXRLoader : MonoBehaviour
{
    [Header("XR Rig Prefab to Spawn")]
    public GameObject xrRigPrefab;

    [Header("Spawn Point (Optional)")]
    public Transform spawnPoint;

    private void Start()
    {

        if (xrRigPrefab == null)
        {
            Debug.LogError("[StandaloneXRLoader] XR Rig prefab not assigned.");
            return;
        }

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject rig = Instantiate(xrRigPrefab, spawnPos, spawnRot);
        rig.name = "Local_XR_Rig";

        Debug.Log("[StandaloneXRLoader] XR Rig spawned successfully.");
    }
}
