using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCrossBow : MonoBehaviour
{
    private CrossbowAnimations crossbowAnimations;

    // When hammer hits crossbow, reload
    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.gameObject.CompareTag("Crossbow"))
        {
            Shooting shooting = collision.gameObject.GetComponent<Shooting>();
            //crossbowAnimations.ReloadAnimation();
            shooting.canFire = true;
            Debug.Log("Crossbow reloaded");
        }
    }
}
