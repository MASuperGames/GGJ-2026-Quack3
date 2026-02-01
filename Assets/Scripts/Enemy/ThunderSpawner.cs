using UnityEngine;

public class ThunderSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;
    [SerializeField] private float localRadius;
    [SerializeField] private GameObject thunder;

    [SerializeField] private float spawnInterval;

    private float lastSpawned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawned = -Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        if (Vector3.Distance(player.transform.position, transform.position) > spawnRadius) return;
        if (lastSpawned + spawnInterval > Time.time) return;
        lastSpawned = Time.time;
        Vector3 ws;
        do
        {
            Vector2 point = localRadius * Random.insideUnitCircle;
            ws = player.transform.TransformPoint(new Vector3(point.x, 0, point.y));
        } while(Vector3.Distance(ws, transform.position) > spawnRadius);
        Instantiate(thunder, ws, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
