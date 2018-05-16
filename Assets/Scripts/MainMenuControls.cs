using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControls : MonoBehaviour {

    void Start() {
        GameParams.GamePlayState = GamePlayState.Menu;
    }

    public void ExitPressed()
    {
        //Debug.Log("Exit pressed!");
        Application.Quit();
    }
}
