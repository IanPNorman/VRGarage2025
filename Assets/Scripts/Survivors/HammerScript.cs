using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HammerScript : MonoBehaviour
{
    int numHits = 0;

    public HealthHandler healthHandler;
    // Repair door on collision
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Door")
        {
            if(numHits == 3)
            {
                healthHandler.HealthChanged(1);
                numHits = 0;
            }
            numHits++;
        }
    }


}
