using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SingleHandGrab : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private bool isHeld = false;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isHeld)
        {
            
            grab.interactionManager.CancelInteractableSelection((UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)grab);
        }
        else
        {
            isHeld = true;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }
}
