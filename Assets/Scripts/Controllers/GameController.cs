using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Menu;
    [SerializeField] public GameOverType CurrentGameOverType { get; private set; } = GameOverType.None;
    public int Score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        AudioController.Instance.PlayMusic(SoundType.StartGame);
    }

    // Cambia el estado y realiza acciones asociadas.
    private void SetState(GameState newState)
    {
        CurrentState = newState;
        // TODO: notificar UIController
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
        //ScoreController.Instance.ResetScore();
        Debug.Log("Load Scene");
        SceneController.Instance.LoadGameScene();
    }

    public void PauseGame()
    {
        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public void FinishGame(GameOverType gameOverType)
    {
        CurrentGameOverType = gameOverType;
        Score = ScoreController.Instance.GetScore(); // Obtener el puntaje actual
        SetState(GameState.GameOver); // Cambiar el estado a GameOver
        SceneController.Instance.LoadGameOverScene();
    }

    public void ResetGame()
    {
        // Recargar escena y restaurar valores
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
