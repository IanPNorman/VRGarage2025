using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowAnimations : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Play firing animation
    public void FireAnimation()
    {
        if(anim != null)
        {
            anim.Play("Base Layer.StylizedCrossbowRig|Firing", 0);
            Debug.Log("Crossbow fired");
        }
    }

    // Play reload animation and return to Idle
    public void ReloadAnimation()
    {
        if (anim != null)
        {
            anim.Play("Base Layer.StylizedCrossbowRig|Reloading", 0);
            Debug.Log("Crossbow reloaded");
            StartCoroutine(ReturnToIdleAfterReload());
        }
    }

    private IEnumerator ReturnToIdleAfterReload()
    {
        // Get the length of the Reloading animation
        float reloadLength = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(reloadLength);

        // Play Idle animation after reload is done
        anim.Play("Base Layer.StylizedCrossbowRig|IdleReady", 0);
    }
}
