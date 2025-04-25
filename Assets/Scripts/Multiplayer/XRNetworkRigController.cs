using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRNetworkRigController : NetworkBehaviour
{
    public GameObject localOnlyObjects; // Ray Interactors, UI, callouts, etc.
    public GameObject visualsToHideFromOwner; // Head/hand models

    public PlayerInput playerInput; // From Unity's Input System

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // This is the local XR rig
            localOnlyObjects?.SetActive(true);

            // Optionally hide your own head/hands from self
            visualsToHideFromOwner?.SetActive(false);
        }
        else
        {
            // This is another player's XR rig — disable all interactive input
            playerInput.enabled = false;

            // OPTIONAL: also disable interactors
            localOnlyObjects?.SetActive(false);
        }
    }
}
