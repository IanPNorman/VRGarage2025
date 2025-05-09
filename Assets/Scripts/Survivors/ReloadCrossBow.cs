using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCrossBow : MonoBehaviour
{
    private Shooting shooting;
    private CrossbowAnimations crossbowAnimations;

    // When hammer hits crossbow, reload
    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.gameObject.CompareTag("Crossbow"))
        {
            crossbowAnimations.ReloadAnimation();
            shooting.canFire = true;
        }
    }
}
