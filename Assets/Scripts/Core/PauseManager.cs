using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseManager : Singleton<PauseManager>
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private GameObject blur;

    private bool isPaused = false;

    private void Start()
    {

    }

    private void OnEnable()
    {
        inputReader.PauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        inputReader.PauseEvent -= TogglePause;
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        statusText.text = "Paused";

        blur.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        statusText.text = "Unpaused";
        blur.SetActive(false);
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}