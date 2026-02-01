using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject thunderSpawner;

    public void onStageChange(int stage)
    {
        if (stage == 2)
        {
            enemySpawner.SetActive(false);
            thunderSpawner.SetActive(true);
        }

        if (stage == 3)
        {
            enemySpawner.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
