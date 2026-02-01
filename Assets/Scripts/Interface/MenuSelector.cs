using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Image[] selectors;
    [SerializeField] private MainMenu mainMenu;

    [Header("Audio")]
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip confirmClip;

    private int selectedIndex = 0;

    private void OnEnable()
    {
        inputReader.NavigateEvent += HandleNavigate;
        inputReader.SubmitEvent += HandleSubmit;
    }

    private void OnDisable()
    {
        inputReader.NavigateEvent -= HandleNavigate;
        inputReader.SubmitEvent -= HandleSubmit;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectors[selectedIndex].enabled = true;
        for (int i = 1; i < selectors.Length; i++)
        {
            selectors[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleNavigate(Vector2 input)
    {
        AudioManager.Instance.PlaySFX(selectClip, 0.2f);

        if (input.y > 0.5f) // Up
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = selectors.Length - 1;
            UpdateSelector();
        }
        else if (input.y < -0.5f) // Down
        {
            selectedIndex++;
            if (selectedIndex >= selectors.Length)
                selectedIndex = 0;
            UpdateSelector();
        }
    }

    private void UpdateSelector()
    {
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i].enabled = (i == selectedIndex);
        }
    }

    private void HandleSubmit()
    {
        switch (selectedIndex)
        {
            case 0:
                AudioManager.Instance.PlaySFXWithPitchVariation(confirmClip, 2.0f);
                StartCoroutine(StartGame());
                break;
            case 1:
                AudioManager.Instance.PlaySFXWithPitchVariation(selectClip, 1.0f);
                mainMenu.OnSettingsClicked();
                break;
            case 2:
                AudioManager.Instance.PlaySFXWithPitchVariation(selectClip, 1.0f);
                mainMenu.OnQuitClicked();
                break;
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.75f);
        mainMenu.OnPlayClicked();
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            AudioManager.Instance.PlaySFX(clip);
        }
    }
}
