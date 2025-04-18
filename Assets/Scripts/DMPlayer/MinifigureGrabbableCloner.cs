using UnityEngine;

public class MinifigureGrabbableCloner : MonoBehaviour
{
    public MinifigureSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand")) // Optional: refine as needed
        {
            GameObject clone = spawner.SpawnClone();

            // Optional: Give it a small push toward the hand
            if (clone.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(Vector3.forward * 0.1f, ForceMode.Impulse);
            }
        }
    }
}
