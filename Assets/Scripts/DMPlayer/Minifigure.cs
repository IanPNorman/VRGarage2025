using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Minifigure : MonoBehaviour
{
    public static event System.Action<Minifigure> OnGrabbed;
    public static event System.Action<Minifigure> OnReleased;
    public GameObject enemyPrefab;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private BoardSlot currentNearbySlot = null;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => OnGrabbed?.Invoke(this));
        grab.selectExited.AddListener(OnReleasedHandler);
    }

    private void OnReleasedHandler(SelectExitEventArgs args)
    {
        OnReleased?.Invoke(this);

        if (currentNearbySlot != null && !currentNearbySlot.isFilled)
        {
            currentNearbySlot.TryAssignFigure(this);
            grab.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.TryGetComponent(out BoardSlot slot))
    {
        currentNearbySlot = slot;
        slot.ShowHoverHighlight(true);
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.TryGetComponent(out BoardSlot slot) && slot == currentNearbySlot)
    {
        slot.ShowHoverHighlight(false);
        currentNearbySlot = null;
    }
}


    public void SnapToSlot(Vector3 slotPosition)
{
    // Optional: adjust Y offset based on your model height
    Vector3 finalPosition = slotPosition + new Vector3(0, 0.05f, 0);

    transform.position = finalPosition;
    transform.rotation = Quaternion.identity;

    // Disable physics to stop flying/rolling
    if (TryGetComponent(out Rigidbody rb))
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Lock interaction
    if (TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab))
    {
        grab.enabled = false;
    }
}

}
