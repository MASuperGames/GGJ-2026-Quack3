using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorPlayModeLoadFirstScene
{
    private static string previousScenePath;

    static EditorPlayModeLoadFirstScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // Save what scene we were working on
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