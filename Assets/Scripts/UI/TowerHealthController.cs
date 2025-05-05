using UnityEngine;
using UnityEngine.UI;

// La lógica de este script es muy parecida a HUDController, pero en vez de usar Player, usa Tower
// quizás la podría unificar en una clase base, pero no es necesario por ahora
public class TowerHealthController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        // Inicializa el slider con la salud máxima
        healthSlider.maxValue = Tower.FindObjectOfType<Tower>().MaxHealth;
        healthSlider.value    = Tower.FindObjectOfType<Tower>().CurrentHealth;

        // Suscribir al evento
        Tower.OnHealthChanged += UpdateHealthBar;
    
        healthSlider.fillRect.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f, 1f); // Verde
    }

    private void OnDestroy()
    {
        Tower.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthSlider.value = currentHealth;

        if (currentHealth <= 400f)
        {
            healthSlider.fillRect.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f, 1f); // Rojo
        }
    }
}
