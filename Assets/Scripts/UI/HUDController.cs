using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Lives UI")]
    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private Sprite fullLifeSprite;
    [SerializeField] private Sprite emptyLifeSprite;

    private float maxHealth;
    private int maxLives;

    private void Awake()
    {
        Player.OnHealthChanged += UpdateHealthBar;
        Player.OnLivesChanged += UpdateLivesIcons;
    }

    private void OnDestroy()
    {
        Player.OnHealthChanged -= UpdateHealthBar;
        Player.OnLivesChanged -= UpdateLivesIcons;
    }

    private void Start()
    {
        var player = FindObjectOfType<Player>();
        // Inicializar la barra de salud
        maxHealth = player.MaxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        // Inicializar iconos de vidas
        maxLives = player.MaxLives;

        UpdateLivesIcons(maxLives);
        healthSlider.fillRect.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f, 1f); // Verde
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthSlider.value = currentHealth;

        if(currentHealth <= 30f){
            healthSlider.fillRect.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f, 1f); // Rojo
        } else {
            healthSlider.fillRect.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f, 1f); // Verde
        }
    }

    private void UpdateLivesIcons(int currentLives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].sprite = (i < currentLives) ? fullLifeSprite : emptyLifeSprite;
        }
    }
}
