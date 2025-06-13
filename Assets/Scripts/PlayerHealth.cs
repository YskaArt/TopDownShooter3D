using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDied;


    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject brokenPrefab;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Curado! Salud actual: " + currentHealth);
        UpdateHealthUI();
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Instantiate(brokenPrefab, transform.position, transform.rotation);
        Debug.Log("El jugador ha muerto.");
        Destroy(gameObject);
        
    }
}
