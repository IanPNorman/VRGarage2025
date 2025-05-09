using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;
 
public class Shooting : MonoBehaviour
{
    public Transform FirePoint;
    //public InputActionReference triggerAction;
    private GameObject objectHit;
    private CrossbowAnimations crossbowAnimations;
    public bool canFire;

    [SerializeField] private AudioClip shootSound;
    [SerializeField][Range(0f, 1f)] private float shootVolume;
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(Shoot);

        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        //crossbowAnimations = GetComponent<CrossbowAnimations>();
    }
    
    // Update is called once per frame
    void Update()
    {
        /*
        if(triggerAction != null && triggerAction.action.WasPressedThisFrame())
        {
            Shoot();
            Debug.Log("Trigger pressed");
        }*/
    }

    public void Shoot()
    {
        // Only shoot if we can fire
        if(!canFire)
        {
            return;
        }

        RaycastHit hit;
        HealthHandler healthHandler;

        if (Physics.Raycast(FirePoint.position, FirePoint.transform.TransformDirection(Vector3.forward), out hit, 200) && canFire)
        {
            Debug.DrawRay(FirePoint.position, FirePoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

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

        // Play sound effect
        if(shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound, shootVolume);
            Debug.Log("playing crossbow shoot sound");
        }
        else
        {
            Debug.LogWarning("Missing shoot sound or AudioSource component");
        }

        //crossbowAnimations.FireAnimation();
        //canFire = false;
    }
}
