using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    [SerializeField] private float timeout = 10.0f;

    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + timeout < Time.time)
        {
            SceneManager.LoadScene(0);
        }
    }
}
