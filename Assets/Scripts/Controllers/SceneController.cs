using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGameScene()
    {
        LoadScene("Game");
    }

    public void LoadIntroScene()
    {
        LoadScene("IntroScene");
    }

    public void LoadGameOverScene()
    {
        LoadScene("GameOver");
    }

    public void LoadHowToPlayScene()
    {
        LoadScene("HowToPlay");
    }

    public void LoadMenuScene()
    {
        LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }



    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
