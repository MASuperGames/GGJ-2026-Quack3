using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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

        for (int i = 0; i < selectors.Length; i++)
        {
            int index = i;
            AddMouseHoverListener(selectors[i].gameObject, index);
        }
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

    private void AddMouseHoverListener(GameObject obj, int index)
    {
        // Add EventTrigger component if it doesn't exist
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        // Create PointerEnter event
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { OnMouseHover(index); });
        trigger.triggers.Add(pointerEnter);

        // Create PointerClick event for mouse click support
        EventTrigger.Entry pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;
        pointerClick.callback.AddListener((data) => { OnMouseClick(index); });
        trigger.triggers.Add(pointerClick);
    }

    private void OnMouseHover(int index)
    {
        if (selectedIndex != index)
        {
            selectedIndex = index;
            AudioManager.Instance.PlaySFX(selectClip, 0.2f);
            UpdateSelector();
        }
    }

    private void OnMouseClick(int index)
    {
        selectedIndex = index;
        UpdateSelector();
        HandleSubmit();
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
                mainMenu.OnCreditsClicked();
                break;
            case 3:
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
