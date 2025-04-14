using UnityEngine;


public class Minifigure : MonoBehaviour
{
    public delegate void MinifigureGrabbedEvent(Minifigure figure);
    public static event MinifigureGrabbedEvent OnGrabbed;
    public static event MinifigureGrabbedEvent OnReleased;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener((_) => OnGrabbed?.Invoke(this));
        grab.selectExited.AddListener((_) => OnReleased?.Invoke(this));
    }
}
