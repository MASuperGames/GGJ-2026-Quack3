using UnityEngine;

public class DefaultCam : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.UnlockCursorEvent += OnUnlockCursor;
    }

    private void OnDisable()
    {
        inputReader.UnlockCursorEvent -= OnUnlockCursor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.enabled = GameObject.FindGameObjectsWithTag("Player").Length == 0;
    }

    private void OnUnlockCursor(bool isPressed)
    {
        if (isPressed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
