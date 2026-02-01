using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private void Start()
    {
        inputReader.EnableUIInput();
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