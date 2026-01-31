using UnityEngine;

public class RoidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roidPrefab;
    [SerializeField] private float respawnTimeout = 5;

    private bool seenDespawn = false;
    private float despawnTime;


    void instantiateRoid() {
        Instantiate(roidPrefab, transform);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instantiateRoid();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0 && !seenDespawn)
        {
            seenDespawn = true;
            despawnTime = Time.time;   
        }

        if (transform.childCount == 0 && despawnTime + respawnTimeout < Time.time)
        {
            instantiateRoid();
            seenDespawn = false;
        }
    }
}
