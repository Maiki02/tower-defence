using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenuController : MonoBehaviour
{
    

    public void BackToMenu()
    {
        //Debug.Log("Regresando al menú principal...");
        GameController.Instance.ResetGame();
    }
}
