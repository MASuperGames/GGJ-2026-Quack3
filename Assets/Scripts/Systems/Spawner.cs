using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [Header("Player Spawning")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private bool destroyOnRespawn = false;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 spawnAreaCenter = Vector3.zero;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private int maxSpawnAttempts = 30;
    [SerializeField] private float enemyRadius = 0.5f;
    [SerializeField] private LayerMask obstacleLayer;

    public UnityEvent<GameObject> onPlayerSpawned;
    private GameObject spawnedPlayer;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned!");
            return;
        }

        if (playerSpawnPoint == null)
        {
            Debug.LogWarning("Player spawn point is not assigned! Spawning at Spawner position.");
        }

        // Get spawn position and rotation
        Vector3 spawnPosition = playerSpawnPoint != null ? playerSpawnPoint.position : transform.position;
        Quaternion spawnRotation = playerSpawnPoint != null ? playerSpawnPoint.rotation : transform.rotation;

        // Check if player already exists
        if (spawnedPlayer != null)
        {
            if (destroyOnRespawn)
            {
                // Destroy and create new instance
                Destroy(spawnedPlayer);
                spawnedPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation);
                Debug.Log("Player destroyed and respawned at: " + spawnPosition);
            }
            else
            {
                // Just reposition existing player
                spawnedPlayer.transform.position = spawnPosition;
                spawnedPlayer.transform.rotation = spawnRotation;

                // Reset velocity if using CharacterController
                CharacterController cc = spawnedPlayer.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    cc.enabled = true;
                }

                Debug.Log("Player repositioned at: " + spawnPosition);
            }
        }
        else
        {
            // No existing player, spawn new one
            spawnedPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Debug.Log("Player spawned at: " + spawnPosition);
        }

        onPlayerSpawned?.Invoke(spawnedPlayer);
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        Vector3 spawnPosition;
        bool foundValidPosition = false;

        // Try to find a valid spawn position
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Generate random position within spawn area
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

            spawnPosition = spawnAreaCenter + new Vector3(randomX, spawnHeight, randomZ);

            // Check if position is valid (no collision with walls/obstacles)
            if (!Physics.CheckSphere(spawnPosition, enemyRadius, obstacleLayer))
            {
                // Valid position found!
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"Enemy spawned at: {spawnPosition}");
                foundValidPosition = true;
                break;
            }
        }

        if (!foundValidPosition)
        {
            Debug.LogWarning($"Failed to find valid spawn position after {maxSpawnAttempts} attempts!");
        }
    }

    public GameObject GetSpawnedPlayer()
    {
        return spawnedPlayer;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}