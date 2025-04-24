using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HammerScript : MonoBehaviour
{
    int numHits = 0;
    TriggerHaptic triggerHaptic;
    
    // Repair door on collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) // If hammer hits door
        {
            // Get HealthHandler and DoorHealth from the door being hit
            HealthHandler healthHandler = other.GetComponent<HealthHandler>();
            DoorHealth doorHealth = other.GetComponent<DoorHealth>();

            numHits++;

            triggerHaptic.HapticFeedback(0.5f, 0.2f);

            if (numHits >= 3) // Heal door after 3 hits
            {
                if (healthHandler.CurrentHealth < healthHandler.MaxHealth) // Check health is not full
                {
                    healthHandler.HealthChanged(1); // Increase door health

                    // Repair barricade
                    GameObject repairable = doorHealth.getRepairable((int)healthHandler.CurrentHealth - 1);
                    GameObject barricade = doorHealth.getBarricade((int)healthHandler.CurrentHealth - 1);

                    if (repairable != null)
                    {
                        repairable.SetActive(false);
                    }
                    if (barricade != null)
                    {
                        barricade.SetActive(true);
                    }
                }

                numHits = 0;
            }
        }
    }
}
    

    
