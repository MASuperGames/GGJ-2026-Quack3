using UnityEngine;

public class Roid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60;
    [SerializeField] public float healthAmount = 30;

    public void PickUp()
    {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime, Space.World);
    }
}
