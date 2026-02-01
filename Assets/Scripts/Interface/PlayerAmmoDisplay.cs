using System;
using TMPro;
using UnityEngine;

public class PlayerAmmoDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private TextMeshProUGUI ammoText;

    private FirstPersonCombat currentPlayerFPSCombat;

    private void Awake()
    {
        spawner.onPlayerSpawned.AddListener(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        // Unsubscribe from old player
        if (currentPlayerFPSCombat != null)
        {
            currentPlayerFPSCombat.onAmmoChange.RemoveListener(OnAmmoChanged);
        }

        // Subscribe to new player
        currentPlayerFPSCombat = player.GetComponent<FirstPersonCombat>();
        if (currentPlayerFPSCombat != null)
        {
            currentPlayerFPSCombat.onAmmoChange.AddListener(OnAmmoChanged);

            // Update UI immediately
            Debug.Log("new subscribe: " + currentPlayerFPSCombat.getCurrentAmmo());
            OnAmmoChanged(currentPlayerFPSCombat.getCurrentAmmo());
        }
    }

    private void OnAmmoChanged(int newAmmo)
    {
        Debug.Log("new ammo : " + newAmmo);
        if (ammoText != null)
        {
            ammoText.text = $"x {newAmmo}";
        }
    }
}
