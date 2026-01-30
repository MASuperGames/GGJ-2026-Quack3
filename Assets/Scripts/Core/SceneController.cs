using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private bool isLoading = false;

    // Load by name
    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    // Load by index
    public void LoadScene(int sceneIndex)
    {
        if (!isLoading)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if next scene exists in build settings
        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene! This is the last scene in Build Settings.");
        }
    }

    // Async load (for loading screens)
    public void LoadSceneAsync(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }
    }

    public void ReloadCurrentScene()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadScene(currentScene);
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        isLoading = true;

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            // Optional: Update loading bar with asyncLoad.progress
            yield return null;
        }

        isLoading = false;
    }
}