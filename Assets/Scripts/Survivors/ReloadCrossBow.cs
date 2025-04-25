using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCrossBow : MonoBehaviour
{
    Shooting shooting;
    CrossbowAnimations crossbowAnimations;

    // When hammer hits crossbow, reload
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Crossbow"))
        {
            crossbowAnimations.ReloadAnimation();
            shooting.canFire = true;
        }
    }
}
