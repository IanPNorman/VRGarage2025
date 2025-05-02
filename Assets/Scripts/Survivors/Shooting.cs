using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

public class Shooting : MonoBehaviour
{
    public Transform FirePoint;
    public InputActionReference triggerAction;
    private GameObject objectHit;
    CrossbowAnimations crossbowAnimations;
    public bool canFire;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(triggerAction != null && triggerAction.action.WasPressedThisFrame())
        {
            Shoot();
            Debug.Log("Trigger pressed");
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        HealthHandler healthHandler;

        if (Physics.Raycast(FirePoint.position, transform.TransformDirection(Vector3.forward), out hit, 100) && (canFire = true))
        {
            Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            objectHit = hit.collider.gameObject;
            Debug.Log("Hit" + objectHit);

            if (objectHit != null && objectHit.CompareTag("Enemy"))
            {
                healthHandler = objectHit.GetComponent<HealthHandler>();
                if (healthHandler != null)
                {
                    healthHandler.HealthChanged(-1);
                    Debug.Log("Enemy health -1");
                }
            }
            
        }

        crossbowAnimations.FireAnimation();
        canFire = false;
    }
}
