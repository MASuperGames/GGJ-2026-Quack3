using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private float dps = 30;

    [SerializeField] private bool damageEnabled = false;

    private float startedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageEnabled)
        {
            var res = Physics.OverlapSphere(transform.position, 5);
            foreach (var col in res)
            {
                if (col.tag != "Player") continue;
                col.GetComponent<HealthManager>()?.ChangeHealth(-dps * Time.deltaTime);
            }
        }

        if (startedTime + 8.0f < Time.time)
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
