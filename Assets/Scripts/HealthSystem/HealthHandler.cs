using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{

    [SerializeField] float maxHealth = 4;

    public delegate void ChangeHealth(object source, float oldHealth, float newHealth);
    public event ChangeHealth OnHealthChanged;

    [SerializeField] float currentHealth;

    public float CurrentHealth => currentHealth;


    public void HealthChanged(float amount)
    {
        float oldHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);


        OnHealthChanged?.Invoke(this, oldHealth, currentHealth);

    }

    public void fullHeal()
    {
        float oldHealth = currentHealth;
        currentHealth = maxHealth;

        OnHealthChanged?.Invoke(this, oldHealth, currentHealth);
    }

    public void instantKill()
    {
        float oldHealth = currentHealth;
        currentHealth = 0;

        OnHealthChanged?.Invoke(this, oldHealth, currentHealth);
    }

    void Start()
    {
        OnHealthChanged += HandleHealthChange;

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            HealthChanged(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            HealthChanged(-1);
        }

    }

    private void HandleHealthChange(object source, float oldHealth, float newHealth)
    {
        Debug.Log($"Health changed from {oldHealth} to {newHealth}.");

    }

}
