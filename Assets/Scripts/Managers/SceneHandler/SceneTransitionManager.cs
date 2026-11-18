using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{

    public Image fadeImage; // Assign a full-screen UI Image in the Inspector
    private const float FadeDuration = 0.1f;


    public void TransitionToScene(SceneName scene)
    {
        string sceneName = SceneHelper.GetSceneName(scene);
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(FadeAndLoad(sceneName));
        }
        else
        {
            Debug.LogError("Scene name is invalid!");
        }
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade to black (skip the fade when no fade image is assigned, e.g. when the manager was auto-created)
        if (fadeImage != null)
        {
            for (float t = 0; t < FadeDuration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0, 1, t / FadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Fade from black (skip the fade when no fade image is assigned, e.g. when the manager was auto-created)
        if (fadeImage != null)
        {
            for (float t = 0; t < FadeDuration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1, 0, t / FadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }
}