using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportFaceBoard : MonoBehaviour
{
    [Header("Target the player should face")]
    public Transform boardCenter;

    [Header("The XR Rig root (e.g., XR Origin)")]
    public Transform xrRigTransform;

    [Header("Ray-based Teleport Interactor")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor teleportInteractor;

    private void OnEnable()
    {
        if (teleportInteractor != null)
        {
            teleportInteractor.selectExited.AddListener(OnTeleportComplete);
            Debug.Log("[TeleportFaceBoard] Subscribed to teleport ray interactor event.");
        }
        else
        {
            Debug.LogWarning("[TeleportFaceBoard] teleportInteractor not assigned!");
        }
    }

    private void OnDisable()
    {
        if (teleportInteractor != null)
        {
            teleportInteractor.selectExited.RemoveListener(OnTeleportComplete);
        }
    }

    private void OnTeleportComplete(SelectExitEventArgs args)
    {
        Debug.Log("[TeleportFaceBoard] Teleport event received! Rotating...");

        if (boardCenter == null || xrRigTransform == null)
        {
            Debug.LogWarning("TeleportFaceBoard: Missing references.");
            return;
        }

        Vector3 direction = boardCenter.position - xrRigTransform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            xrRigTransform.forward = direction.normalized;
            Debug.Log("TeleportFaceBoard: Player rotated to face the board.");
        }
    }
}
