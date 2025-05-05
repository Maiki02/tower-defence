using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DamageFlashController : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.5f;
    [Range(0,1)][SerializeField] private float maxAlpha = 0.5f;

    private Image img;
    private Coroutine flashCoroutine;

    void Awake()
    {
        img = GetComponent<Image>();
        // Asegurarnos de empezar invisible
        img.color = new Color(1f, 0f, 0f, 0f);

        // Suscribirse al evento de daño del player
        Player.OnHealthChanged += HandleHealthChanged;
    }

    void OnDestroy()
    {
        Player.OnHealthChanged -= HandleHealthChanged;
    }

    private float lastHealth = -1f;
    private void HandleHealthChanged(float currentHealth)
    {
        // Si bajó la salud (recibió daño), lanzamos el flash
        if (lastHealth >= 0 && currentHealth < lastHealth)
            Flash();
        lastHealth = currentHealth;
    }

    public void Flash()
    {
        // Reinicia la corrutina si ya estaba en curso
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Poner alfa inicial
        img.color = new Color(1f, 0f, 0f, maxAlpha);

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / flashDuration);
            img.color = new Color(1f, 0f, 0f, maxAlpha * t);
            yield return null;
        }

        // Asegurar que quede invisible al final
        img.color = new Color(1f, 0f, 0f, 0f);
    }
}
