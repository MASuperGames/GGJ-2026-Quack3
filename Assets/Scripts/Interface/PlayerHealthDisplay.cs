using System;
using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private TextMeshProUGUI healthText;

    private HealthManager currentPlayerHealth;

    private void Start()
    {
        spawner.onPlayerSpawned.AddListener(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        // Unsubscribe from old player
        if (currentPlayerHealth != null)
        {
            currentPlayerHealth.onHealthChange.RemoveListener(OnHealthChanged);
            currentPlayerHealth.onHealthDepleted.RemoveListener(OnPlayerDeath);
        }

        // Subscribe to new player
        currentPlayerHealth = player.GetComponent<HealthManager>();
        if (currentPlayerHealth != null)
        {
            currentPlayerHealth.onHealthChange.AddListener(OnHealthChanged);
            currentPlayerHealth.onHealthDepleted.AddListener(OnPlayerDeath);

            // Update UI immediately
            OnHealthChanged(currentPlayerHealth.Health, 0);
        }
    }

    private void OnHealthChanged(float newHealth, float delta)
    {
        if (healthText != null)
        {
            healthText.text = $"{(int)Math.Round(newHealth)}";
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player died!");

        // Update UI to show death
        if (healthText != null)
        {
            healthText.text = "R.I.P.";
        }
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.onPlayerSpawned.RemoveListener(OnPlayerSpawned);
        }

        // Clean up
        if (currentPlayerHealth != null)
        {
            currentPlayerHealth.onHealthChange.RemoveListener(OnHealthChanged);
            currentPlayerHealth.onHealthDepleted.RemoveListener(OnPlayerDeath);
        }
    }
}
