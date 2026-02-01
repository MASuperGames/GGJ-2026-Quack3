using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Roid,
        Feather,
        BoneFragment,
    }

    [SerializeField] public Type itemType;

    [SerializeField] private float rotationSpeed = 60;
    [SerializeField] public float healthAmount = 30;

    private Animator animator;
    private MeshCollider myCollider;
    private Rigidbody rb;

    private float pickedUpTime = Mathf.Infinity;

    public void PickUp()
    {
        GetComponent<MeshCollider>().enabled = false;
        if (rb != null) rb.isKinematic = true;
        GetComponent<Animator>().SetTrigger("FadeOut");
        pickedUpTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime, Space.World);

        if (pickedUpTime + 1.0f < Time.time)
            Destroy(gameObject);
    }
}
