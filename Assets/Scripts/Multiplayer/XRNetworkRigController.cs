using Unity.Netcode;
using UnityEngine;

public class XRNetworkRigController : NetworkBehaviour
{
    [Header("Local-only Systems")]
    public GameObject[] localOnlyObjects;

    [Header("Optional - Main Camera Listener")]
    public AudioListener audioListener;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(true);
        }
        else
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(false);

            if (audioListener != null)
                audioListener.enabled = false;
        }
    }
}
