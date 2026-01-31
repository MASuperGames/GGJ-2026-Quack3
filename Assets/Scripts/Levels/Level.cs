using UnityEngine;

public class Level01 : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private int numEnemies = 10;
    [SerializeField] private GameObject duckEnemy;
    [SerializeField] private GameObject shootEnemy;

    void Start()
    {
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
