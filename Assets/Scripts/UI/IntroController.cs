using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;     // duración del fade

    void Start()
    {
        // Empieza transparente
        canvasGroup.alpha = 0;
        StartCoroutine(FadeIn());
    }

    public void OnSkipPressed()
    {
        // Detiene cualquier fade en curso y empieza el FadeOut
        StopAllCoroutines();
        StartCoroutine(FadeOutThenLoad());
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    private IEnumerator FadeIn()
    {
        return Fade(0, 1);
    }

    private IEnumerator FadeOutThenLoad()
    {
        yield return Fade(1, 0);
        SceneController.Instance.LoadGameScene();
    }
}
