using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject credits;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool fadeInMusic = false;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        inputReader.EnableUIInput();
        credits.SetActive(false);
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

    public void OnCreditsClicked()
    {
        credits.SetActive(true);
    }    

    public void OnQuitClicked()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame && credits.activeSelf)
        {
            credits.SetActive(false);
        }
    }
}