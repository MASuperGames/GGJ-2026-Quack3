using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;
    [SerializeField] private float localRadius;
    [SerializeField] private float playerMinDistance;
    [SerializeField] private float duckFraction;
    [SerializeField] private GameObject duck;

    [SerializeField] private GameObject bone;

    [SerializeField] private float spawnInterval;

    [SerializeField] private float navMeshSearchRadius = 2f;
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;

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
        do {
            Vector2 point = localRadius * Random.insideUnitCircle;
            ws = player.transform.TransformPoint(new Vector3(point.x, 0, point.y));
        } while(
            Vector3.Distance(ws, transform.position) > spawnRadius ||
            Vector3.Distance(ws, player.transform.position) < playerMinDistance
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(ws, out hit, navMeshSearchRadius, navMeshAreaMask))
        {
            ws = hit.position;
            GameObject prefab = Random.value < duckFraction ? duck : bone;
            if (prefab == duck) ws += new Vector3(0, 2 + Random.value, 0);
            Instantiate(prefab, ws, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
