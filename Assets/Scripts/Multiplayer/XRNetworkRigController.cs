using Unity.Netcode;
using UnityEngine;

public class XRNetworkRigController : NetworkBehaviour
{
    [Header("Local-only Systems (disabled for remote)")]
    public GameObject[] localOnlyObjects;

    //[Header("Visuals to hide for self (hands/head)")]
    //public GameObject[] hideFromOwnerVisuals;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(true);

            //foreach (var go in hideFromOwnerVisuals)
                //if (go != null) go.SetActive(false); // hide own hands/head if desired
        }
        else
        {
            foreach (var go in localOnlyObjects)
                if (go != null) go.SetActive(false);
        }
        
    }
}
