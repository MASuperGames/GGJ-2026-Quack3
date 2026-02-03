using UnityEngine;
using UnityEngine.Events;

public class Player : Singleton<Player>
{
    [SerializeField] private float roidPickupRadius;

    private HealthManager healthManager;
    private FirstPersonController fpc;
    private FirstPersonCombat fpcom;

    public int numFeathers = 0;
    public int numBoneFragments = 0;
    public int numZombieFleshFragments = 0;

    public UnityEvent<int> onFeatherCollected;
    public UnityEvent<int> onBoneFragmentCollected;
    public UnityEvent<int> onZombieFleshFragmentCollected;

    public void InvokeItemCollectedEvents()
    {
        Debug.Log("Invoking item collected events");
        onFeatherCollected.Invoke(numFeathers);
        onBoneFragmentCollected.Invoke(numBoneFragments);
        onZombieFleshFragmentCollected.Invoke(numZombieFleshFragments);
    }

    public void onHealthChange(float health, float delta)
    {
    }

    public void onHealthDepleted()
    {
        //Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        fpc = GetComponent<FirstPersonController>();
        fpcom = GetComponent<FirstPersonCombat>();

        onFeatherCollected.Invoke(0);
        onBoneFragmentCollected.Invoke(0);
        onZombieFleshFragmentCollected.Invoke(0);
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
                    onFeatherCollected.Invoke(numFeathers);
                    if (numFeathers < 4) break;
                    fpc.featherMode = true;
                    break;
                case Item.Type.BoneFragment:
                    numBoneFragments++;
                    onBoneFragmentCollected.Invoke(numBoneFragments);
                    if (numBoneFragments < 4) break;
                    fpcom.boneFragmentMode = true;
                    break;
                case Item.Type.Roid:
                    healthManager.ChangeHealth(item.healthAmount);
                    numZombieFleshFragments++;
                    onZombieFleshFragmentCollected.Invoke(numZombieFleshFragments);
                    if (numZombieFleshFragments < 4) break;
                    fpcom.zombieFleshmode = true;
                    break;
                case Item.Type.Ammo:
                    fpcom.AddAmmo(item.ammoAmount);
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
