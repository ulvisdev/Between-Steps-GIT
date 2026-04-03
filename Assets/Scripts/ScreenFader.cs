using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeOut()
    {
        float i = 0f;

        canvasGroup.blocksRaycasts = true;

        while (i < fadeDuration)
        {
            i += Time.deltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, i / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeIn()
    {
        float i = 0f;

        while (i < fadeDuration)
        {
            i += Time.deltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(1f, 0f, i / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}