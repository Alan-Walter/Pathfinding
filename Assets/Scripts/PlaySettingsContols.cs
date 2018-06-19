using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Класс, отвечающий за настройки игры, сцены
/// </summary>
public class PlaySettingsContols : MonoBehaviour {
    #region Объеты интерфейса Юнити
    public GameObject GenerationConfig;
    public GameObject LoadMap;
    public Dropdown SelectMap;
    public GameObject SizeInfo;
    public InputField SizeInputField;
    public Dropdown GameModeDropdown;
    public Dropdown MapTypeDropdown;
    #endregion

    void Awake() {
        //  установка дефолтных значений
        GameParams.GetMapsNames();
        GameParams.GameMode = GameModes.Sandbox;
        GameParams.CameraState = CameraStates.Normal;
        GameParams.MapType = MapTypes.Generation;
        GameParams.GamePlayState = GamePlayState.Menu;
        GameParams.MapScale = 1.0f;
        GameParams.Seed = 1.0f;
        GameParams.MapId = 0;

        GameParams.Width = GameConstants.MinWidth;
        GameParams.Height = GameConstants.MaxHeight;
        GameParams.Length = GameConstants.MinLength;
    }
    //  Нажатие клавиши старт
    public void StartPressed() {
        GameParams.Length = GameParams.Width = int.Parse(SizeInputField.text);
        GameParams.GameMode = (GameModes)GameModeDropdown.value;
        GameParams.MapType = (MapTypes)MapTypeDropdown.value;
        SceneManager.LoadScene("main");
    }
    //  Изменение режима игры
    public void OnGameModeChanged(int value) {
        GameParams.GameMode = (GameModes)value;
    }
    //  Изменение типа карты
    public void OnMapTypeChanged(int value) {
        GameParams.MapType = (MapTypes)value;
        if(GameParams.MapType == MapTypes.Generation)
        {
            GenerationConfig.SetActive(true);
            LoadMap.SetActive(false);
            SizeInfo.SetActive(true);
        }
        else if(GameParams.MapType == MapTypes.DefaultMaps)
        {
            SelectMap.AddOptions(GameParams.MapNames);
            GenerationConfig.SetActive(false);
            LoadMap.SetActive(true);
            SizeInfo.SetActive(false);
        }
        else
        {
            GenerationConfig.SetActive(false);
            LoadMap.SetActive(false);
            SizeInfo.SetActive(true);
        }
        OnSizeInputFieldChange(SizeInputField.text);
        OnSizeInputFieldEndEdit("");
    }
    //  Изменение масштаба
    public void OnScaleChanged(float value) {
        GameParams.MapScale = value;
    }
    //  Изменение смещения
    public void OnSeedChanged(float value) {
        GameParams.Seed = value;
    }
    //  Выбор карты
    public void OnSelectMap(int value) {
        GameParams.MapId = value;
    }
    //  Изменение размера карты
    public void OnSizeInputFieldChange(string text) {
        if (!int.TryParse(text, out GameParams.Width) || GameParams.Width < GameConstants.MinWidth)
            GameParams.Width = GameConstants.MinWidth;
        else if (GameParams.MapType == MapTypes.Labyrinth && GameParams.Width > GameConstants.MazeMaxWidth)
            GameParams.Width = GameConstants.MazeMaxWidth;
        else if (GameParams.Width > GameConstants.MaxWidth)
            GameParams.Width = GameConstants.MaxWidth;
    }

    public void OnSizeInputFieldEndEdit(string text) {
        SizeInputField.text = GameParams.Width.ToString();
    }
}
