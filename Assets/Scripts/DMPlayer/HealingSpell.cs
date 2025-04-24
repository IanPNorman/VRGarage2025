using UnityEngine;


public class HealingSpell : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [Header("Prefabs")]
    public GameObject healingZonePrefab;

    [Header("Ground Detection")]
    public LayerMask groundLayer;

    [Header("Optional VFX Cleanup Delay")]
    public float destroyDelay = 0f;

    [Header("Mana Settings")]
    public int manaCost = 3;

    private bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        // Check if it hit the ground
        if (((1 << collision.gameObject.layer) & groundLayer.value) != 0)
        {
            if (ManaManager.Instance != null && ManaManager.Instance.SpendMana(manaCost))
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
            else
            {
                Debug.Log("Not enough mana to activate healing spell.");
            }
        }
    }

    // Block grabbing if player can't afford it
    public override bool IsSelectableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor)
    {
        return ManaManager.Instance != null &&
               ManaManager.Instance.GetCurrentMana() >= manaCost &&
               base.IsSelectableBy(interactor);
    }
}
