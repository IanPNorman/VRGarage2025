using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BookAutoReturn : MonoBehaviour
{
    [Header("Holster reference")]
    public Transform holsterTransform;

    [Header("Auto-return delay (seconds)")]
    public float returnDelay = 1.5f;

    [Header("Visual components to hide/show")]
    public GameObject bookVisuals;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    private float releaseTime;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        if (bookVisuals != null)
        bookVisuals.SetActive(true);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        releaseTime = Time.time;
        if (bookVisuals != null)
        bookVisuals.SetActive(false);
    }

    private void Update()
    {
        if (!isHeld && Time.time - releaseTime >= returnDelay)
        {
            ReturnToHolster();
        }
    }

    private void ReturnToHolster()
    {
        if (holsterTransform == null) return;

        // Move the book back and reset physics
        transform.position = holsterTransform.position;
        transform.rotation = holsterTransform.rotation;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("[BookAutoReturn] Returned to holster.");
    }
}
