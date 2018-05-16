using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMenuControls : MonoBehaviour {

    public static bool IsPlayerSpawnUnit { get; set; }
    public static bool IsPlayerSpawnBuild { get; set; }

    public ButtonKeyStates UnitButtonState { get; private set; }
    public ButtonKeyStates BuildButtonState { get; private set; }
    public bool IsSpawnMenuButtonChecked
    {
        get
        {
            return UnitButtonState == ButtonKeyStates.Checked || BuildButtonState == ButtonKeyStates.Checked;
        }
    }

    public Button UnitButton;
    public Button BuildButton;
    public Button NextButton;

    private Image unitButtonImage;
    private Image buildButtonImage;

    public GameObject UnitPrefab;
    public GameObject BuildPrefab;

    public GameObject SpawnMenu;

    void Start() {
        unitButtonImage = UnitButton.GetComponent<Image>();
        buildButtonImage = BuildButton.GetComponent<Image>();
        if (GameParams.GameMode == GameModes.Sandbox)
        {
            SpawnMenu.SetActive(true);
            NextButton.gameObject.SetActive(false);
        }
        else SpawnMenu.SetActive(false);
    }

    void Update() {
        if (GameParams.GamePlayState != GamePlayState.Spawn || !Input.GetMouseButton(1)) return;
        RaycastHit hit = SelectGameObject.GetHitFromCursor();
        if (hit.transform == null || hit.transform.tag != "Terrain") return;
        Vector2Int spawnPos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
        if (TerrainNavGrid.Instance.IsCellUsed(spawnPos)) return;
        GameObject target;
        if (UnitButtonState == ButtonKeyStates.Checked)
        {
            if (GameParams.GameMode == GameModes.Competitions && Vector2Int.Distance(spawnPos, CompetitionMenuContols.StartGridPosition) > GameConstants.FlagRadius)
                TipsControls.Instance.SetTipsText("Невозможно спавнить юнита за пределами зоны старта!");
            else
            {
                target = Instantiate(UnitPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
                IsPlayerSpawnUnit = GameParams.GameMode == GameModes.Competitions;
            }
        }
        else
        {
            target = Instantiate(BuildPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
            IsPlayerSpawnBuild = GameParams.GameMode == GameModes.Competitions;
        }
        if (GameParams.GameMode == GameModes.Competitions)
            ResetButtonsStates();

    }

    public void OnUnitSpawnButtonClick() {
        if (GameParams.GamePlayState == GamePlayState.End) return;
        if (GameParams.GameMode == GameModes.Competitions && IsPlayerSpawnUnit)
        {
            TipsControls.Instance.SetTipsText("Невозможно спавнить больше 1 юнита за ход!");
            return;
        }
        if (UnitButtonState == ButtonKeyStates.Normal)
        {
            if (BuildButtonState == ButtonKeyStates.Checked)
                SetBuildButtonState(ButtonKeyStates.Normal);
            SetUnitButtonState(ButtonKeyStates.Checked);
        }
        else
            SetUnitButtonState(ButtonKeyStates.Normal);
    }

    public void OnBuildSpawnButtonClick() {
        if (GameParams.GamePlayState == GamePlayState.End) return;
        if (GameParams.GameMode == GameModes.Competitions && IsPlayerSpawnBuild)
        {
            TipsControls.Instance.SetTipsText("Невозможно строить больше 1 здания за ход!");
            return;
        }
        if (BuildButtonState == ButtonKeyStates.Normal)
        {
            if (UnitButtonState == ButtonKeyStates.Checked)
                SetUnitButtonState(ButtonKeyStates.Normal);
            SetBuildButtonState(ButtonKeyStates.Checked);
        }
        else
            SetBuildButtonState(ButtonKeyStates.Normal);
    }

    private void SetUnitButtonState(ButtonKeyStates state) {
        UnitButtonState = state;
        if (UnitButtonState == ButtonKeyStates.Normal)
            unitButtonImage.color = UnitButton.colors.normalColor;
        else if (UnitButtonState == ButtonKeyStates.Checked)
            unitButtonImage.color = UnitButton.colors.pressedColor;
        ButtonsStateChanges();
    }

    private void SetBuildButtonState(ButtonKeyStates state) {
        BuildButtonState = state;
        if (BuildButtonState == ButtonKeyStates.Normal)
            buildButtonImage.color = BuildButton.colors.normalColor;
        else if (BuildButtonState == ButtonKeyStates.Checked)
            buildButtonImage.color = BuildButton.colors.pressedColor;
        ButtonsStateChanges();
    }

    private void ResetButtonsStates() {
        SetUnitButtonState(ButtonKeyStates.Normal);
        SetBuildButtonState(ButtonKeyStates.Normal);
    }

    private void ButtonsStateChanges()
    {
        if (IsSpawnMenuButtonChecked)
            GameParams.GamePlayState = GamePlayState.Spawn;
        else GameParams.GamePlayState = GamePlayState.Play;
    }

    public void OnNextButtonClick()
    {
        if (GameParams.GamePlayState == GamePlayState.End) return;
        PlayerController.Instance.NextPlayer();
    }
}
