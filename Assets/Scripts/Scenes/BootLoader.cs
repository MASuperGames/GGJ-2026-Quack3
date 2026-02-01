using System.Collections;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float bootDelay = 0.5f;
    [SerializeField] private float soundDelay = 0.5f;
    [SerializeField] private AudioClip clip;

    private void Start()
    {

        InitializeManagers();
        StartCoroutine(PlaySplashSound());
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

    private IEnumerator PlaySplashSound()
    {
        yield return new WaitForSeconds(soundDelay);

        AudioManager.Instance.PlaySFXWithPitchVariation(clip, 1.0f, 1.0f, 0.5f);
    }

    private IEnumerator LoadFirstScene()
    {
        yield return new WaitForSeconds(bootDelay);

        // Load main menu
        SceneController.Instance.LoadNextScene();
    }
}