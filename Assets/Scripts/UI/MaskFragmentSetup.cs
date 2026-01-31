using UnityEngine;

public class MaskFragmentSetup : MonoBehaviour
{
    [SerializeField] private MaskUI maskFeather;
    [SerializeField] private MaskUI maskBoneFragments;
    [SerializeField] private MaskUI maskZombieFleshFragments;


    public void OnPlayerSpawned(GameObject go)
    {
        var player = go.GetComponent<Player>();
        player.onFeatherCollected.AddListener(maskFeather.SetCount);
        player.onBoneFragmentCollected.AddListener(maskBoneFragments.SetCount);
        player.onZombieFleshFragmentCollected.AddListener(maskZombieFleshFragments.SetCount);
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
