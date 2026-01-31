using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float roidPickupRadius;

    private HealthManager healthManager;

    public void onHealthChange(float health, float delta)
    {
    }

    public void onHealthDepleted()
    {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var res = Physics.OverlapSphere(transform.position, roidPickupRadius);
        foreach (var col in res)
        {
            var roid = col.GetComponent<Roid>();
            if (roid == null)
                continue;
            healthManager.ChangeHealth(roid.healthAmount);
            roid.PickUp();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, roidPickupRadius);        
    }
}
