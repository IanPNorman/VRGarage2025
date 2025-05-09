using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HealingSpell : XRGrabInteractable
{
    [Header("Prefabs")]
    public GameObject healingZonePrefab;

    [Header("Optional VFX Cleanup Delay")]
    public float destroyDelay = 0f;

    [Header("Tag used to detect floor")]
    public string groundTag = "Ground";

    private bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
{
    //Debug.Log($"[HealingSpell] Collided with: {collision.gameObject.name}, tag: {collision.gameObject.tag}");

    if (hasLanded) return;

    if (collision.gameObject.CompareTag(groundTag))
    {
        //Debug.Log("[HealingSpell] Ground detected — casting healing zone.");
        hasLanded = true;
        Instantiate(healingZonePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, destroyDelay > 0f ? destroyDelay : 0f);
    }
}
}
