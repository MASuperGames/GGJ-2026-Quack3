using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool fadeInMusic = false;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        inputReader.EnableUIInput();

        // Start background music
        if (backgroundMusic != null)
        {
            if (fadeInMusic)
            {
                AudioManager.Instance.FadeInMusic(backgroundMusic, fadeDuration);
            }
            else
            {
                AudioManager.Instance.PlayMusic(backgroundMusic);
            }
        }
    }

    public void OnPlayClicked()
    {
        inputReader.EnableGameplayInput();
        SceneController.Instance.LoadNextScene();
    }

    public void OnSettingsClicked()
    {
        // Open settings panel
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}