using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject credits;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;
    [SerializeField] private Button thirdButton;
    [SerializeField] private Button fourthButton;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip playClip;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool fadeInMusic = false;
    [SerializeField] private float fadeDuration = 2f;

    private GameObject lastSelected;

    public void PlaySelectSound()
    {
        AudioManager.Instance.PlaySFX(selectClip);
    }

    private void Start()
    {
        //inputReader.EnableUIInput();
        credits.SetActive(false);
        firstButton.Select();
        lastSelected = firstButton.gameObject;
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
        AudioManager.Instance.PlaySFX(playClip);
        inputReader.EnableGameplayInput();
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadNextScene();
        }
        else
        {
            Debug.Log("SceneController instance not found!");
        }

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
        if (Keyboard.current.escapeKey.wasPressedThisFrame && credits.activeSelf)
        {
            credits.SetActive(false);
        }

        GameObject currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (currentSelected == null)
        {
            // Silently restore selection WITHOUT triggering sound
            if (lastSelected != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(lastSelected);
                // Don't update lastSelected or play sound - we're just restoring
            }
        }
        else if (currentSelected != lastSelected)
        {
            // Selection changed by user action (hover or keyboard)
            lastSelected = currentSelected;
            PlaySelectSound();
            Debug.Log("Currently selected: " + currentSelected.name);
        }
    }
}