using System.Collections;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float bootDelay = 0.5f; // Optional: show logo/splash

    private void Start()
    {
        // Optional: Initialize other systems here
        InitializeManagers();

        // Load first real scene
        StartCoroutine(LoadFirstScene());
    }

    private void InitializeManagers()
    {
        // Force managers to initialize by accessing them
        // This ensures they're created before first scene loads
        var audio = AudioManager.Instance;
        var scenes = SceneController.Instance;

        Debug.Log("Managers initialized!");
    }

    private IEnumerator LoadFirstScene()
    {
        // Optional: Show splash screen, company logo, etc.


        yield return new WaitForSeconds(bootDelay);

        // Load main menu
        SceneController.Instance.LoadNextScene();
    }
}