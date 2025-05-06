using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using Unity.XR.CoreUtils;

public class HealthBar : MonoBehaviour
{
    [SerializeField] MicroBar healthBar;
    public HealthHandler healthHandler;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.Initialize(healthHandler.MaxHealth);
    }

    // Damage player and change health bar
    public void Damage(float damage)
    {
        float damageAmount = damage;
        healthBar.UpdateBar(healthBar.CurrentValue - damageAmount);
        healthHandler.HealthChanged(-damageAmount);
    }
}
