using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Roid,
        Feather
    }

    [SerializeField] public Type itemType;

    [SerializeField] private float rotationSpeed = 60;
    [SerializeField] public float healthAmount = 30;

    private Animator animator;
    private MeshCollider myCollider;

    private float pickedUpTime = Mathf.Infinity;

    public void PickUp()
    {
        myCollider.enabled = false;
        animator.SetTrigger("FadeOut");
        pickedUpTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        myCollider = GetComponent<MeshCollider>();
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime, Space.World);

        if (pickedUpTime + 1.0f < Time.time)
            Destroy(gameObject);
    }
}
