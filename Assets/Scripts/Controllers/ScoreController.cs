using System;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    // Singleton para acceso fácil desde cualquier parte del juego
    public static ScoreController Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore = 0;

    private void Awake()
    {
        // Inicialización del singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Asegurarse de que la UI arranque en 0
        UpdateScoreText();
    }

    /// Suma puntos al score actual y notifica.
    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        currentScore += amount;
        UpdateScoreText();
    }

    /// Resetea el score a cero (por ejemplo, al reiniciar partida).
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    // Actualiza la etiqueta en pantalla.
    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString("0");
    }

    public int GetScore()
    {
        return currentScore;
    }
}
