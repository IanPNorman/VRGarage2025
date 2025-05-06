using UnityEngine;

public class TeleportPointVisibility : MonoBehaviour
{
    [Header("Player XR Rig Root (e.g., XR Origin)")]
    public Transform playerRig;

    [Header("Distance to hide teleport point")]
    public float hideRadius = 0.5f;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (playerRig == null) return;

        float distance = Vector3.Distance(transform.position, playerRig.position);

        bool shouldBeVisible = distance > hideRadius;

        foreach (var rend in renderers)
        {
            if (rend != null)
                rend.enabled = shouldBeVisible;
        }
    }
}
