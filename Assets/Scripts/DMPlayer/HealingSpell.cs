using UnityEngine;

public class HealingSpell : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject healingZonePrefab;

    [Header("Ground Detection")]
    public LayerMask groundLayer;

    [Header("Optional VFX Cleanup Delay")]
    public float destroyDelay = 0f; // Set to 0 for instant

    private bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        // Check if collided object is on the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer.value) != 0)
        {
            hasLanded = true;

            // Spawn the healing zone at impact point
            Instantiate(healingZonePrefab, transform.position, Quaternion.identity);

            // Destroy the spell object immediately (or after delay)
            if (destroyDelay > 0f)
                Destroy(gameObject, destroyDelay);
            else
                Destroy(gameObject);
        }
    }
}
