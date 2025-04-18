using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportFaceBoard : MonoBehaviour
{
    [Header("Target the player should face")]
    public Transform boardCenter;

    [Header("The XR Rig root (e.g., XR Origin)")]
    public Transform xrRigTransform;

    [Header("Teleportation Provider from Locomotion > Teleportation")]
    public TeleportationProvider tpProvider;

    [Header("Optional: Delay before turning")]
    public float rotationDelay = 0f;

    private void OnEnable()
    {
        if (tpProvider != null)
        {
            Debug.Log("[TeleportFaceBoard] Subscribed to teleport event.");
            tpProvider.endLocomotion += OnTeleportEnd;
            
        }
        else
        {
            Debug.LogWarning("TeleportationProvider is not assigned.");
        }
    }

    private void OnDisable()
    {
        if (tpProvider != null)
        {
            tpProvider.endLocomotion -= OnTeleportEnd;
        }
    }

    private void OnTeleportEnd(LocomotionSystem system)
    {
        Debug.Log("TeleportFaceBoard: Teleport completed.");

        if (boardCenter == null || xrRigTransform == null)
        {
            Debug.LogWarning("TeleportFaceBoard: Missing boardCenter or xrRigTransform reference.");
            return;
        }

        if (rotationDelay > 0f)
        {
            Invoke(nameof(RotateTowardBoard), rotationDelay);
        }
        else
        {
            RotateTowardBoard();
        }
    }

    private void RotateTowardBoard()
    {
        Vector3 direction = boardCenter.position - xrRigTransform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            xrRigTransform.forward = direction.normalized;
            Debug.Log("TeleportFaceBoard: Player rotated to face the board.");
        }
        else
        {
            Debug.Log("TeleportFaceBoard: Direction was too small, skipping rotation.");
        }
    }
}
