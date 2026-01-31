using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10;
    [SerializeField] private float maxLifeTime = 10;

    private float spawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * speed * transform.forward;

        var res = Physics.OverlapSphere(transform.position, 0.3f);
        bool anythingHit = false;
        foreach (var col in res)
        {
            if (col.gameObject != gameObject)
                anythingHit = true;
            if (col.tag == "Player")
                col.GetComponent<HealthManager>()?.ChangeHealth(-damage);
        }

        if (anythingHit || spawnTime + maxLifeTime < Time.time)
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
