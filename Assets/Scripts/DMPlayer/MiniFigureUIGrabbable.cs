using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class MiniFigureUISpawner : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [Header("Prefab to spawn when grabbed")]
    public GameObject miniFigurePrefab;

    protected override void Awake()
    {
        base.Awake();
        // Fully lock this object in place
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Spawn the clone
        GameObject clone = Instantiate(miniFigurePrefab, transform.position, transform.rotation);

        // Transfer the grab to the clone
        var interactor = args.interactorObject;
        if (clone.TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable))
        {
            interactionManager.SelectEnter(interactor, grabInteractable);
        }

        // Immediately force the original (UI) object to deselect and stay in place
        interactionManager.SelectExit(interactor, this);
    }

    // Prevent accidental deselection effects
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Ensure the image doesn’t move or act up
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
