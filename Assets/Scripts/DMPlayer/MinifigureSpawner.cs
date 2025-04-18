using UnityEngine;


public class MinifigureSpawner : MonoBehaviour
{
    public GameObject minifigurePrefab;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Immediately disable direct interaction
        grab.enabled = false;

        
        if (TryGetComponent(out Collider col))
            col.enabled = false;
    }

    // Called externally to trigger a spawn (e.g. from a trigger volume or animation)
    public GameObject SpawnClone()
    {
        GameObject clone = Instantiate(minifigurePrefab, transform.position, transform.rotation);
        return clone;
    }

    
}
