using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using Unity.XR.CoreUtils;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private MicroBar healthBar;
    public HealthHandler healthHandler;

    [Header("XR Rig Reference")]
    [SerializeField] private GameObject xrRig; 
    [SerializeField] private string tagToSetOnDeath = "Dead"; 

    void Start()
    {
        healthBar.Initialize(healthHandler.MaxHealth);
        healthHandler.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(object source, float oldHealth, float newHealth)
    {
        healthBar.UpdateBar(newHealth);

        if (newHealth <= 0f)
        {
            xrRig.tag = tagToSetOnDeath;

        }
    }

    private void OnDestroy()
    {
        if (healthHandler != null)
            healthHandler.OnHealthChanged -= HandleHealthChanged;
    }
}
