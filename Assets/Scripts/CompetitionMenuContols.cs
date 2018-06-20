using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за режим соревнования
/// </summary>
public class CompetitionMenuContols : MonoBehaviour {
    #region Поля
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
    #endregion

    /// <summary>
    /// Активация режима соревнования
    /// </summary>
    void Start() {
        if (GameParams.GameMode == GameModes.Competitions)
        {
            CompetitionMenu.SetActive(true);
            GameParams.GamePlayState = GamePlayState.SelectParams;
        }
        else CompetitionMenu.SetActive(false);
    }

    // Update is called once per frame
    /// <summary>
    /// Отслеживание состояний соревнования.
    /// </summary>
    void Update () {
		if((GameParams.GamePlayState == GamePlayState.SelectStart || GameParams.GamePlayState == GamePlayState.SelectFinish) 
            && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = SelectGameObject.GetHitFromCursor();
            if (hit.transform == null || hit.transform.tag != "Terrain") return;
            Vector2Int spawnPos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z)); 
            //  получение координат по щелчку мыши
            if (TerrainNavGrid.Instance.IsCellUsed(spawnPos)) return;
            if (GameParams.GamePlayState == GamePlayState.SelectStart)  //  если состояние - выбор старта
            {
                SpawnStart(spawnPos);
                //  спавн объекта зоны старта
            }
            else  //  если состояние - выбор финиша
            {
                float distance = Vector2Int.Distance(StartGridPosition, spawnPos);
                if (distance <= GameConstants.MaxDistance)
                {
                    TipsControls.Instance.SetTipsText("Слишком близко к зоне старта!");
                    return;
                }
                PathFinder finder = new PathFinder(StartGridPosition, spawnPos, GameConstants.PathFindMinHeight);
                finder.FindPath();
                if(!finder.IsPathFound)
                {
                    TipsControls.Instance.SetTipsText("Невозможно найти путь между стартом и финишом!");
                    return;
                }
                SpawnFinish(spawnPos);
                //  спавн объекта зоны финиша
            }
            GameParams.GamePlayState = GamePlayState.SelectParams;
            TipsControls.Instance.SetTipsText("");
        }
	}

    /// <summary>
    /// Метод спавна зоны старта
    /// </summary>
    /// <param name="spawnPos">Координаты на навигационной сетке</param>
    private void SpawnStart(Vector2Int spawnPos) {
        if (startObject != null)
            Destroy(startObject.gameObject);
        if (finishObject != null)
            Destroy(finishObject.gameObject);
        finishObject = null;
        startObject = Instantiate(StartPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
        StartGridPosition = spawnPos;
    }

    /// <summary>
    /// Метод спавна зоны финиша
    /// </summary>
    /// <param name="spawnPos">Координаты спавна на навигационной сетке</param>
    private void SpawnFinish(Vector2Int spawnPos) {
        if (finishObject != null)
            Destroy(finishObject.gameObject);
        finishObject = Instantiate(FinishPrefab, new Vector3(spawnPos.x, 0, spawnPos.y), new Quaternion()) as GameObject;
        FinishGridPosition = spawnPos;
    }

    /// <summary>
    /// Метод нажатия кнопки спавна
    /// </summary>
    public void OnStartSpawnButtonClick() {
        if (GameParams.GamePlayState != GamePlayState.SelectParams) return;
        GameParams.GamePlayState = GamePlayState.SelectStart;
        TipsControls.Instance.SetTipsText("Выбор зоны старта");
    }

    /// <summary>
    /// Метод нажатия кнопки финиша
    /// </summary>
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

    /// <summary>
    /// Метод нажатия кнопки play
    /// </summary>
    public void OnPlayButtonClick() {
        if (GameParams.GamePlayState != GamePlayState.SelectParams) return;
        if(startObject == null || finishObject == null)
        {
            TipsControls.Instance.SetTipsText("Сначала установите флаги старта и финиша!");
            return;
        }
        CompetitionMenu.SetActive(false);
        SpawnMenu.SetActive(true);
        GameParams.GamePlayState = GamePlayState.Play;
        TipsControls.Instance.SetPlayerInfo();
    }
}
