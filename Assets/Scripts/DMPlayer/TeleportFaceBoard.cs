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
    public TeleportationProvider tpProvider;  // ✅ assign this manually in Inspector

    private void OnEnable()
    {
        if (tpProvider != null)
        {
            tpProvider.endLocomotion += OnTeleportEnd;
        }
        else
        {
            Debug.LogWarning("TeleportFaceBoard: TeleportationProvider is not assigned.");
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
    Debug.Log("Teleport complete: rotating toward board");

    if (boardCenter == null || xrRigTransform == null)
    {
        Debug.LogWarning("TeleportFaceBoard: Missing references.");
        return;
    }

    Vector3 direction = boardCenter.position - xrRigTransform.position;
    direction.y = 0f;

    Debug.Log("Direction to board: " + direction);

    if (direction.sqrMagnitude > 0.001f)
    {
        xrRigTransform.forward = direction.normalized;
        Debug.Log("Player rotated to face the board.");
    }
    else
    {
        Debug.Log("Look direction was too small. No rotation applied.");
    }
}

}
