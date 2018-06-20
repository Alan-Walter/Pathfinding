using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class Unit : MonoBehaviour, ISelectObject, INavGridSpawn, INavGridMove, IFindPath {

    public PathImage PathImage { get; private set; }  //  изображения пути движения

    public int StepCount { get; private set; }  //  кол-во шагов

    public Player Player { get; private set; }  //  владелец юнита

    private cakeslice.Outline outline;

    private bool isGameObjectMove = false;  //  передвигается ли игровой объект юнита
    private bool isPositionBlocked = false;  //  позиция пути заблокирована

    private Vector2Int gridPosition;  //  позиция на навигационной сетке

    private Vector2Int gridNextPosition;  //  следующая позиция движения по навигационной сетке
    private Vector3 nextPosition;  //  следующая позиция в трёхмерном пространстве

    private Vector2Int gridFinishPosition;  //  финишная позиция на сетке

    private List<Vector2Int> movePath;  //  список позиций движения

    private float maxHeight = 2;  //  максимальная высота, на которую может подняться юнит
    private float speed = 10.2f;  //  скорость передвижения

    private TargetPoint targetPoint;  //  флаг, к которому закреплён юнит

    private Timer timer;  //  таймер юнита

    private int attemptCount = GameConstants.MaxPathFindAttempt;  //  кол-во попыток найти путь до финиша

    public float MaxHeight {
        get
        {
            return maxHeight;
        }
    }

    public Vector2Int GridPosition {
        get
        {
            return gridPosition;
        }
    }

    public PathFinder PathFind { get; set; }  //  поиск пути

    // инициализация объекта
    void Start () {
        UnitManager.Instance.AddUnit(this);
        SpawnObject(new Vector2Int(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)));
        outline = this.gameObject.GetComponentInChildren<cakeslice.Outline>();
        this.Player = PlayerController.ActivePlayer;
        foreach (var x in this.GetComponentInChildren<Renderer>().materials)
            x.color = Player.Color;
        timer = this.gameObject.AddComponent<Timer>();
        timer.onElapsed = TimerElapsed;
    }
	
	// передвижения игрового объекта по карте
	void Update () {
        if (!isGameObjectMove) return;
        Vector3 position = this.gameObject.transform.position;
        if (Mathf.RoundToInt(position.x) != Mathf.RoundToInt(nextPosition.x) || Mathf.RoundToInt(position.z) != Mathf.RoundToInt(nextPosition.z))
            MoveGameObjectToPoint();
        else
            isGameObjectMove = false;
    }

    //  получение координат движения
    void FixedUpdate() {
        if (isPositionBlocked) return;
        if (!isGameObjectMove && movePath != null && movePath.Count > 0)
        {
            if (GameParams.GameMode == GameModes.Competitions && StepCount == GameConstants.MaxStepPoints)
                StopMove();
            else
            if (this.gridPosition != gridFinishPosition)
            {
                //  если текущая позиция не равна позиции финиша
                if (IsGetNextPoint())  //  если можно получить следующую позицию, то переходим на неё
                {
                    isGameObjectMove = GetNextPoint();
                    if (isGameObjectMove) MoveGrid();
                    return;
                }  //  если юнит заблокирован со всех сторон, то включается таймер ожидания
                else if (!isPositionBlocked && PathFinder.IsPositionBlocked(movePath[movePath.Count - 1]))
                {
                    isPositionBlocked = true;
                    timer.SetTimer(1);
                    return;
                }
                //  начинается поиск нового пути до финиша
                StopMove();
                SetMovePosition(gridFinishPosition);
            }
        }
    }

    //  при уничтожении удаляется из множества юнитов
    void OnDestroy() {
        if(UnitManager.Instance != null)
            UnitManager.Instance.DeleteUnit(this);
    }

    #region Выделение и снятие контура юнита
    public void OnSelectObject() {
        outline.selected = true;
    }

    public void OnDeselectObject()  {
        outline.selected = false;
    }
    #endregion

    //  установка новой цели движения
    public void OnSetTarget(TargetPoint point) {
        if(targetPoint != null)
            targetPoint.DeleteLink();
        if (!TerrainNavGrid.IsPositionCorrect(point.Position)) return;
        StopMove();
        targetPoint = point;
        targetPoint.AddLink();
        PathImage = new PathImage(GameParams.Width, GameParams.Length);
        SetMovePosition(targetPoint.Position);
    }

    //  спавн объекта
    public void SpawnObject(Vector2Int position) {
        gridPosition = position;
        this.gameObject.transform.position =
            new Vector3(position.x, TerrainHeightMap.Instance.GetHeight(position), position.y);
        TerrainNavGrid.Instance.Spawn(gridPosition);
    }

    //  установка позиции движения
    public void SetMovePosition(Vector2Int position) {
        if (GameParams.GameMode == GameModes.Competitions && StepCount == GameConstants.MaxStepPoints) return;
        StopMove();
        timer.StopTimer();
        gridFinishPosition = position;
        if (TerrainNavGrid.Instance.IsCellUsed(gridFinishPosition) && 
            !PathFinder.FindFreeCell(gridFinishPosition, out gridFinishPosition, this.MaxHeight))
                return;  //  если финиш занят и нет близких точек, то return
        TerrainNavGrid.Instance.FindPath(this, gridFinishPosition);
    }

    //  если путь найден, то устанавливаются параметры
    public void OnPathFound(List<Vector2Int> path) {
        movePath = path;
        attemptCount = GameConstants.MaxPathFindAttempt;
        timer.StopTimer();
    }

    //  перемещение юнита по навигационной сетке
    private void MoveGrid() {
        if (GameParams.GameMode == GameModes.Competitions)
            StepCount++;
        TerrainNavGrid.Instance.MoveObject(gridPosition, gridNextPosition);
        gridPosition = gridNextPosition;
        PathImage += gridPosition;
        nextPosition = new Vector3(gridNextPosition.x, TerrainHeightMap.Instance.GetHeight(gridNextPosition), gridNextPosition.y);
    }

    //  получение координат следующей точки движения
    private bool GetNextPoint() {
        if (!IsGetNextPoint()) return false;
        gridNextPosition = movePath[movePath.Count - 1];
        movePath.RemoveAt(movePath.Count - 1);
        return true;
    }

    //  можно ли получить следующую точку
    private bool IsGetNextPoint() {
        return IsPathExists() && !TerrainNavGrid.Instance.IsCellUsed(movePath[movePath.Count - 1]);
    }

    //  путь существует
    private bool IsPathExists() {
        return movePath != null && movePath.Count != 0;
    }

    //  перемещение игрового объекта к точке
    private void MoveGameObjectToPoint() {
        Vector3 position = this.gameObject.transform.position;
        Vector3 temp = nextPosition - position;
        this.gameObject.transform.Translate(new Vector3(temp.x * Time.deltaTime * speed, temp.y * Time.deltaTime * speed,
            temp.z * Time.deltaTime * speed), Space.World);
    }

    //  остановка движения юнита
    private void StopMove() {
        isGameObjectMove = false;
        movePath = null;
        isPositionBlocked = false;
    }

    //  сброс кол-ва шагов
    public void ResetCount() {
        StepCount = 0;
    }

    //  если путь не найден, запускается таймер на повторный поиск пути
    public void OnPathNotFound(FinishNotFoundReasons reason) {
        if (attemptCount == 0 || Vector2Int.Distance(this.gridPosition, gridFinishPosition) <= GameConstants.StopMoveMinDistance) return;
        attemptCount--;
        timer.SetTimer(reason == FinishNotFoundReasons.Blocked ? GameConstants.PathFindBlockedTime : GameConstants.PathFindAttemptTime);
    }

    private void TimerElapsed() {
        if (isPositionBlocked)
            isPositionBlocked = false;
        else
        SetMovePosition(gridFinishPosition);
    }
}
