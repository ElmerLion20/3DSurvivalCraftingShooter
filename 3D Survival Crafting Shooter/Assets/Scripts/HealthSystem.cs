using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    public event EventHandler OnHealed;

    [SerializeField] private int maxHealth;
    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount) {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead()) {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealFull() {
        currentHealth = maxHealth;

        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead() {
        return currentHealth == 0 || currentHealth < 0;
    }

    public bool IsFullHealth() {
        return currentHealth == maxHealth;
    }

    public float GetCurrentHealthAmount() {
        return currentHealth;
    }

    public int GetMaxHealthAmount() {
        return maxHealth;

    }

    public float GetHealthAmountNormalized() {
        return (float)currentHealth / maxHealth;
    }

    public void SetHealthAmountMax(int maxHealthAmount, bool updateHealthAmount) {
        this.maxHealth = maxHealthAmount;

        if (updateHealthAmount) {
            currentHealth = maxHealthAmount;
        }

        OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }

}
