using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorPlayModeLoadFirstScene
{
    private static string previousScenePath;

    private const string MENU_ITEM = "Tools/Auto-Load First Scene";
    private const string PREF_KEY = "AutoLoadFirstScene";

    private static bool AutoLoadEnabled
    {
        get => EditorPrefs.GetBool(PREF_KEY, false);
        set => EditorPrefs.SetBool(PREF_KEY, value);
    }

    static EditorPlayModeLoadFirstScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.delayCall += () => Menu.SetChecked(MENU_ITEM, AutoLoadEnabled);
    }

    [MenuItem(MENU_ITEM)]
    private static void ToggleAutoLoad()
    {
        AutoLoadEnabled = !AutoLoadEnabled;
        Menu.SetChecked(MENU_ITEM, AutoLoadEnabled);
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!AutoLoadEnabled) return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            previousScenePath = EditorSceneManager.GetActiveScene().path;
            SceneManager.LoadScene(0);
            return;
        }
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (!string.IsNullOrEmpty(previousScenePath))
            {
                EditorSceneManager.OpenScene(previousScenePath);
                previousScenePath = null;
            }
        }
    }
}
#endif