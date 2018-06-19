using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за контроль главного меню
/// </summary>
public class MainMenuControls : MonoBehaviour {

    void Start() {
        GameParams.GamePlayState = GamePlayState.Menu;
    }

    public void ExitPressed() {
        Application.Quit();
    }
}
