using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс подсказок на экране
/// </summary>
public class TipsControls : MonoBehaviour {
    public static TipsControls Instance { get; private set; }

    public Text TipsText;
    public GameObject TipsMenu;

    void Start() {
        Instance = this;
        ShowTips(GameParams.GameMode == GameModes.Competitions);
    }

    public void ShowTips(bool state) {
        TipsMenu.SetActive(state);
    }

    public void SetTipsText(string text) {
        TipsText.text = text;
    }

    public void SetPlayerInfo() {
        TipsText.text = string.Format("Ход игрока: {0}", PlayerController.ActivePlayer);
    }
}
