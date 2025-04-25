using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRNetworkRigController : NetworkBehaviour
{
    [Header("Local-only Systems")]
    public GameObject[] localOnlyObjects;

    [Header("Interaction Components to Disable")]
    public ControllerInputActionManager[] inputControllers;
    public XRRayInteractor[] rayInteractors;
    public XRDirectInteractor[] directInteractors;

    [Header("Optional")]
    public AudioListener audioListener;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(false);

            foreach (var ctrl in inputControllers)
                if (ctrl != null) ctrl.enabled = false;

            foreach (var ray in rayInteractors)
                if (ray != null) ray.enabled = false;

            foreach (var direct in directInteractors)
                if (direct != null) direct.enabled = false;

            if (audioListener != null)
                audioListener.enabled = false;
        }
        else
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(true);
        }
    }
}
