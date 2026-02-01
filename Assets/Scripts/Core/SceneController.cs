using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool isLoading = false;

    protected override void Awake()
    {
        base.Awake();

        // Ensure the fade panel starts invisible
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
    }

    // Load by name
    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(FadeAndLoadScene(sceneName));
        }
    }

    // Load by index
    public void LoadScene(int sceneIndex)
    {
        if (!isLoading)
        {
            StartCoroutine(FadeAndLoadScene(sceneIndex));
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene! This is the last scene in Build Settings.");
        }
    }

    public void ReloadCurrentScene()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadScene(currentScene);
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        isLoading = true;

        yield return StartCoroutine(Fade(1f));
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(0f));

        isLoading = false;
    }

    private IEnumerator FadeAndLoadScene(int sceneIndex)
    {
        isLoading = true;

        yield return StartCoroutine(Fade(1f));
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(Fade(0f));

        isLoading = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        float startAlpha = fadeCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}