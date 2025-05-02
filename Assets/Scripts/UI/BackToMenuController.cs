using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuController : MonoBehaviour
{
    

    public void BackToMenu()
    {
        SceneController.Instance.LoadMenuScene();
        GameController.Instance.ResetGame();
    }
}
