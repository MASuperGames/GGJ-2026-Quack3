using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class EnemyWaveGroup
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnDelay = 0.5f;

    public float spawnHeight = 1f;
}

[System.Serializable]
public class Wave
{
    public string waveName = "Wave";
    public List<EnemyWaveGroup> enemyGroups = new List<EnemyWaveGroup>();
    public float waveStartDelay = 2f;
}

public class Level : MonoBehaviour
{
    [SerializeField] private Spawner spawner;


    [Header("Wave Configuration")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private float delayBetweenWaves = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip ambienceSound;
    [SerializeField] private bool fadeInMusic = false;
    [SerializeField] private float fadeDuration = 2f;

    private int currentWaveIndex = 0;
    private bool spawningWaves = false;

    void Start()
    {
        if (backgroundMusic != null)
        {
            if (fadeInMusic)
            {
                AudioManager.Instance.FadeInMusic(backgroundMusic, fadeDuration);
            }
            else
            {
                AudioManager.Instance.PlayMusic(backgroundMusic);
            }
        }

        if (ambienceSound != null)
        {
            AudioManager.Instance.PlayAmbience(ambienceSound);
        }

        // Start spawning waves
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        spawningWaves = true;

        foreach (Wave wave in waves)
        {
            // Wait before starting this wave
            yield return new WaitForSeconds(wave.waveStartDelay);

            Debug.Log($"Starting {wave.waveName}");

            // Spawn each enemy group in the wave
            foreach (EnemyWaveGroup group in wave.enemyGroups)
            {
                spawner.SetEnemyPrefab(group.enemyPrefab);
                spawner.SetSpawnHeight(group.spawnHeight);

                for (int i = 0; i < group.count; i++)
                {
                    spawner.SpawnEnemy();
                    yield return new WaitForSeconds(group.spawnDelay);
                }
            }

            currentWaveIndex++;

            // Wait between waves
            if (currentWaveIndex < waves.Count)
            {
                yield return new WaitForSeconds(delayBetweenWaves);
            }
        }

        spawningWaves = false;
        Debug.Log("All waves completed!");
    }


    void Update()
    {
        
    }
}
