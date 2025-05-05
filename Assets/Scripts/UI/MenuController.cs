using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameController.Instance.StartGame();
    }

    public void HowToPlay()
    {
        SceneController.Instance.LoadHowToPlayScene();
    }

    public void QuitGame()
    {
        SceneController.Instance.QuitGame();
    }
}