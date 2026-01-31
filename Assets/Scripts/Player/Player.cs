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

            var item = col.GetComponent<Item>();
            if (item == null)
                continue;
            switch (item.itemType) {
                case Item.Type.Feather:
                    // TODO
                    break;
                case Item.Type.Roid:
                    healthManager.ChangeHealth(item.healthAmount);
                    break;
            }
            item.PickUp();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, roidPickupRadius);        
    }
}
