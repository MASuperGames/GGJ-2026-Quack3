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
        myCollider.enabled = false;
        if (rb != null) rb.isKinematic = true;
        animator.SetTrigger("FadeOut");
        pickedUpTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        myCollider = GetComponent<MeshCollider>();
        animator = GetComponent<Animator>();        
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
