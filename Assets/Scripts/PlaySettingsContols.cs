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
    public InputField WidthInputField;
    public InputField HeightInputField;

    public void StartPressed()
    {
        //Debug.Log("Start pressed!");
        GameParams.Width = int.Parse(WidthInputField.text);
        GameParams.Length = int.Parse(HeightInputField.text);
        if(GameParams.Length > GameParams.Width)
        {
            int temp = GameParams.Width;
            GameParams.Width = GameParams.Length;
            GameParams.Length = temp;
        }
        SceneManager.LoadScene("main");
    }

    public void OnGameModeChanged(int value)
    {
        GameParams.GameMode = (GameModes)value;
    }

    public void OnMapTypeChanged(int value)
    {
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
                SelectMap.AddOptions(GameParams.MapNames);
            }
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
        OnWidthInputFieldChanged(WidthInputField.text);
        OnHeightInputFieldChanged(HeightInputField.text);
        OnWidthInputFieldEndEdit("");
        OnHeightInputFieldEndEdit("");
    }

    public void OnScaleChanged(float value)
    {
        GameParams.MapScale = value;
    }

    public void OnSeedChanged(float value)
    {
        GameParams.Seed = value;
    }

    public void OnSelectMap(int value)
    {
        GameParams.MapId = value;
    }

    public void OnWidthInputFieldChanged(string text)
    {
        if (!int.TryParse(text, out GameParams.Width) || GameParams.Width < GameConstants.MinWidth)
            GameParams.Width = GameConstants.MinWidth;
        else if (GameParams.MapType == MapTypes.Labyrinth && GameParams.Width > GameConstants.MazeMaxWidth)
            GameParams.Width = GameConstants.MazeMaxWidth;
        else if (GameParams.Width > GameConstants.MaxWidth)
            GameParams.Width = GameConstants.MaxWidth;
    }

    public void OnHeightInputFieldChanged(string text)
    {
        if (!int.TryParse(text, out GameParams.Length) || GameParams.Length < GameConstants.MinLength)
            GameParams.Length = GameConstants.MinLength;
        else if (GameParams.MapType == MapTypes.Labyrinth && GameParams.Length > GameConstants.MazeMaxLength)
            GameParams.Length = GameConstants.MazeMaxLength;
        else if (GameParams.Length > GameConstants.MaxLength)
            GameParams.Length = GameConstants.MaxLength;
    }

    public void OnWidthInputFieldEndEdit(string text)
    {
        WidthInputField.text = GameParams.Width.ToString();
    }

    public void OnHeightInputFieldEndEdit(string text)
    {
        HeightInputField.text = GameParams.Length.ToString();
    }
}
