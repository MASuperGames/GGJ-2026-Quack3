using UnityEngine;

public class Level0 : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private int numEnemies = 10;
    [SerializeField] private GameObject duckEnemy;
    [SerializeField] private GameObject shootEnemy;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip ambienceSound;
    [SerializeField] private bool fadeInMusic = false;
    [SerializeField] private float fadeDuration = 2f;

    void Start()
    {
        // Start background music
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

        // Start ambience (optional)
        if (ambienceSound != null)
        {
            AudioManager.Instance.PlayAmbience(ambienceSound);
        }

        // Spawn enemies of this level
        spawner.SetEnemyPrefab(duckEnemy);
        for (int i = 0; i < numEnemies; i++)
        {
            spawner.SpawnEnemy();
        }
    }
    void Update()
    {
        
    }
}
