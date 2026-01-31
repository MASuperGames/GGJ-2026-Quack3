using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private MaskUI featherMaskUI;

    [SerializeField] private float roidPickupRadius;

    private HealthManager healthManager;
    private FirstPersonController fpc;

    private int numFeathers = 0;

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
        fpc = GetComponent<FirstPersonController>();

        featherMaskUI.SetCount(0);
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
                    numFeathers++;
                    featherMaskUI.SetCount(numFeathers);
                    if (numFeathers < 4) break;
                    fpc.featherMode = true;
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
