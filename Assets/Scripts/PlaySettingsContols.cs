using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySettingsContols : MonoBehaviour {
    public GameObject GenerationConfig;
    public GameObject LoadMap;
    public Dropdown SelectMap;
    public GameObject SizeInfo;
    public InputField SizeInputField;
    public Dropdown GameModeDropdown;
    public Dropdown MapTypeDropdown;

    public void StartPressed() {
        GameParams.Length = GameParams.Width = int.Parse(SizeInputField.text);
        GameParams.GameMode = (GameModes)GameModeDropdown.value;
        GameParams.MapType = (MapTypes)MapTypeDropdown.value;
        SceneManager.LoadScene("main");
    }

    public void OnGameModeChanged(int value) {
        GameParams.GameMode = (GameModes)value;
    }

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
            if(!GameParams.IsDefaultMapsLoaded)
            {
                GameParams.GetMapsNames();
            }
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

    public void OnScaleChanged(float value) {
        GameParams.MapScale = value;
    }

    public void OnSeedChanged(float value) {
        GameParams.Seed = value;
    }

    public void OnSelectMap(int value) {
        GameParams.MapId = value;
    }

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
