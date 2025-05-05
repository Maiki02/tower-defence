using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject towerDeadPanel;
    public GameObject winPanel;
    public GameObject playerDeadPanel;

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private GameOverType gameOverType;
    private void Awake()
    {
        towerDeadPanel.SetActive(false);
        winPanel.SetActive(false);
        playerDeadPanel.SetActive(false);
    }

    private void Start()
    {
        this.gameOverType = GameController.Instance.CurrentGameOverType;
        ShowGameOver(gameOverType);
        this.scoreText.text = "Score: " + ScoreController.Instance.GetScore().ToString("0");
        SetTextByGameOverType(gameOverType);
        PlaySound(gameOverType);
    }

    public void ShowGameOver(GameOverType type)
    {
        switch (type)
        {
            case GameOverType.PlayerWIN:
                winPanel.SetActive(true);
                break;
            case GameOverType.PlayerDEAD:
                playerDeadPanel.SetActive(true);
                break;
            case GameOverType.TowerDEAD:
                towerDeadPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void SetTextByGameOverType(GameOverType type)
    {
        switch (type)
        {
            case GameOverType.PlayerWIN:
                infoText.text = "Contra todo pronóstico, resististe el combate enemigo. Cada victoria ensanchó tu fama, y la torre permanece en pie como testigo de tu fiereza. El reino entona canciones en tu honor, y en las almenas florece la esperanza.";
                break;
            case GameOverType.PlayerDEAD:
                infoText.text = "Tus fuerzas flaquearon y, pese al coraje en tu pecho, el último aliento se extinguió. Tres caídas en combate agotaron tu vida, dejando la leyenda del héroe incompleta.";
                break;
            case GameOverType.TowerDEAD:
                infoText.text = "Lanzaron un asedio implacable. Capturaron a la princesa. El pueblo está abatido, y solo quedan sombras donde antes brillaba la esperanza.";
                break;
            default:
                break;
        }
    }

    private void PlaySound(GameOverType type)
    {
        if(type == GameOverType.TowerDEAD || type == GameOverType.PlayerDEAD)
            AudioController.Instance.PlaySFX(SoundType.Defeat);
        else if(type == GameOverType.PlayerWIN)
            AudioController.Instance.PlaySFX(SoundType.Victory);
    }
}