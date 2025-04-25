using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HealingSpell : XRGrabInteractable
{
    [Header("Prefabs")]
    public GameObject healingZonePrefab;

    [Header("Ground Detection")]
    public LayerMask groundLayer;

    [Header("Optional VFX Cleanup Delay")]
    public float destroyDelay = 0f;

    private bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        // Check if it hit the ground
        if (((1 << collision.gameObject.layer) & groundLayer.value) != 0)
        {
            hasLanded = true;

            // Spawn healing zone
            Instantiate(healingZonePrefab, transform.position, Quaternion.identity);

            // Destroy the ball
            if (destroyDelay > 0f)
                Destroy(gameObject, destroyDelay);
            else
                Destroy(gameObject);
        }
    }
}
