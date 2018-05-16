using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionMenuContols : MonoBehaviour {

    public Button StartSpawnButton;
    public Button FinishSpawnButton;

    public GameObject CompetitionMenu;
    public GameObject SpawnMenu;

    public GameObject StartPrefab;
    public GameObject FinishPrefab;

    private GameObject startObject;
    private GameObject finishObject;

    public static Vector2Int StartGridPosition;
    public static Vector2Int FinishGridPosition;

    void Start() {
        if (GameParams.GameMode == GameModes.Competitions)
        {
            CompetitionMenu.SetActive(true);
            GameParams.GamePlayState = GamePlayState.SelectParams;
        }
        else CompetitionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		if((GameParams.GamePlayState == GamePlayState.SelectStart || GameParams.GamePlayState == GamePlayState.SelectFinish) 
            && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = SelectGameObject.GetHitFromCursor();
            if (hit.transform == null || hit.transform.tag != "Terrain") return;
            Vector2Int spawnPos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
            if (TerrainNavGrid.Instance.IsCellUsed(spawnPos)) return;
            if (GameParams.GamePlayState == GamePlayState.SelectStart)
            {
                if (startObject != null)
                    Destroy(startObject.gameObject);
                if (finishObject != null)
                    Destroy(finishObject.gameObject);
                finishObject = null;
                startObject = Instantiate(StartPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
                StartGridPosition = spawnPos;
            }
            else
            {
                float distance = Vector2Int.Distance(StartGridPosition, spawnPos);
                if (distance <= GameConstants.MaxDistance)
                {
                    TipsControls.Instance.SetTipsText("Слишком близко к зоне старта!");
                    return;
                }
                PathFinder finder = new PathFinder(StartGridPosition, spawnPos, GameConstants.PathFindMinHeight);
                finder.IsActual = true;
                finder.FindPath();
                if(!finder.IsPathFound)
                {
                    TipsControls.Instance.SetTipsText("Невозможно найти путь между стартом и финишом!");
                    return;
                }
                if (finishObject != null)
                    Destroy(finishObject.gameObject);
                finishObject = Instantiate(FinishPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
                FinishGridPosition = spawnPos;
            }
            GameParams.GamePlayState = GamePlayState.SelectParams;
            TipsControls.Instance.SetTipsText("");
        }
	}

    public void OnStartSpawnButtonClick() {
        if (GameParams.GamePlayState != GamePlayState.SelectParams) return;
        GameParams.GamePlayState = GamePlayState.SelectStart;
        TipsControls.Instance.SetTipsText("Выбор зоны старта");

    }

    public void OnFinishSpawnButtonClick() {
        if (GameParams.GamePlayState != GamePlayState.SelectParams) return;
        if (startObject == null)
        {
            TipsControls.Instance.SetTipsText("Первым установите флаг старта!");
            return;
        }
        GameParams.GamePlayState = GamePlayState.SelectFinish;
        TipsControls.Instance.SetTipsText("Выбор зоны финиша");
    }

    public void OnPlayButtonClick() {
        if (GameParams.GamePlayState != GamePlayState.SelectParams) return;
        if(startObject == null || finishObject == null)
        {
            TipsControls.Instance.SetTipsText("Сначала установите флаги старта и финиша!");
            return;
        }
        //TipsControls.Instance.ShowTips(false);
        CompetitionMenu.SetActive(false);
        SpawnMenu.SetActive(true);
        GameParams.GamePlayState = GamePlayState.Play;
        TipsControls.Instance.SetPlayerInfo();
    }
}
