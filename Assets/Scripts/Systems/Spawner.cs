using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(20f, 10f, 20f);
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private int maxSpawnAttempts = 30;
    [SerializeField] private float navMeshSearchRadius = 2f;
    [SerializeField] private float minDistanceBetweenEnemies = 2f;
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;

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
            spawnedPlayer.GetComponent<HealthManager>().onHealthDepleted.RemoveListener(SpawnPlayer);

            if (destroyOnRespawn)
            {
                Debug.Log("destroying...");
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

                // Reset health
                var healthManager = spawnedPlayer.GetComponent<HealthManager>();
                healthManager.Health = healthManager.MaxHealth;
                healthManager.onHealthChange?.Invoke(healthManager.Health, 0);

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
        spawnedPlayer.GetComponent<HealthManager>().onHealthDepleted.AddListener(SpawnPlayer);
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
            float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
            float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

            Vector3 randomPosition = spawnAreaCenter + new Vector3(randomX, randomY, randomZ);

            // Try to find nearest point on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, navMeshSearchRadius, navMeshAreaMask))
            {
                spawnPosition = hit.position;
                spawnPosition.y += spawnHeight;

                // Optional: Check if there's enough space (no other enemies too close)
                if (IsPositionClearOfEnemies(spawnPosition))
                {
                    // Valid position found on NavMesh!
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    //Debug.Log($"Enemy spawned at: {spawnPosition}");
                    foundValidPosition = true;
                    break;
                }
            }
        }

        if (!foundValidPosition)
        {
            Debug.LogWarning($"Failed to find valid spawn position on NavMesh after {maxSpawnAttempts} attempts!");
        }
    }

    private bool IsPositionClearOfEnemies(Vector3 position)
    {
        // Find all NavMeshAgents in the scene
        NavMeshAgent[] existingAgents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in existingAgents)
        {
            float distance = Vector3.Distance(position, agent.transform.position);
            if (distance < minDistanceBetweenEnemies)
            {
                return false; // Too close to another enemy
            }
        }

        return true; // Position is clear
    }

    public GameObject GetSpawnedPlayer()
    {
        return spawnedPlayer;
    }

    public void SetEnemyPrefab(GameObject newPrefab)
    {
        enemyPrefab = newPrefab;
    }

    public void SetSpawnHeight(float height)
    {
        spawnHeight = height;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw spawn area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);

        // Draw NavMesh search radius indicator at center
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnAreaCenter, navMeshSearchRadius);
    }
}