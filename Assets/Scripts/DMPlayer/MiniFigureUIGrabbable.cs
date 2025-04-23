using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniFigureUIGrabbable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [Header("Prefab to Spawn")]
    public GameObject miniFigurePrefab;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (miniFigurePrefab == null)
        {
            Debug.LogWarning("MiniFigureUIGrabbable: No prefab assigned!");
            return;
        }

        // Instantiate a new figure at the hand's attach point
        Transform interactorAttach = args.interactorObject.transform;

        GameObject clone = Instantiate(miniFigurePrefab, interactorAttach.position, interactorAttach.rotation);

        // Grab the newly spawned object automatically
        if (clone.TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable))
        {
            var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor;
            grabInteractable.interactionManager.SelectEnter(interactor, grabInteractable);
        }

        // Destroy the UI clone to prevent it from lingering
        Destroy(gameObject);
    }
}
