using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    public void FinishGame() {
        Debug.Log("Fin del juego");
        SceneController.Instance.LoadGameOverScene();
    }   

    public void ResetGame(){
        Debug.Log("TODO reiniciar valores del juego");

    }
}

