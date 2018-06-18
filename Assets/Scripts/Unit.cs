using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class Unit : MonoBehaviour, ISelectObject, INavGridSpawn, INavGridMove, IFindPath {

    public PathImage PathImage { get; private set; }

    public int StepCount { get; private set; }

    public Player Player { get; private set; }

    private cakeslice.Outline outline;

    private bool isGameObjectMove = false;

    private Vector2Int gridPosition;

    private Vector2Int gridNextPosition;
    private Vector3 nextPosition;

    private Vector2Int gridFinishPosition;

    private List<Vector2Int> movePath;

    private float maxHeight = 2;
    private float speed = 10.2f;

    private TargetPoint targetPoint;

    private Timer timer;

    private int attemptCount = GameConstants.MaxPathFindAttempt;

    public float MaxHeight
    {
        get
        {
            return maxHeight;
        }
    }

    public Vector2Int GridPosition
    {
        get
        {
            return gridPosition;
        }
    }

    public PathFinder PathFind { get; set; }

    // Use this for initialization
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
	
	// Update is called once per frame
	void Update () {
        if (!isGameObjectMove) return;
        Vector3 position = this.gameObject.transform.position;
        if (Mathf.RoundToInt(position.x) != Mathf.RoundToInt(nextPosition.x) || Mathf.RoundToInt(position.z) != Mathf.RoundToInt(nextPosition.z))
            MoveGameObjectToPoint();
        else
            isGameObjectMove = false;
    }

    void FixedUpdate() {
        if (!isGameObjectMove && movePath != null && movePath.Count > 0)
        {
            if (GameParams.GameMode == GameModes.Competitions && StepCount == GameConstants.MaxStepPoints)
                StopMove();
            else
            if (this.gridPosition != gridFinishPosition)
            {
                if (IsGetNextPoint())
                {
                    isGameObjectMove = GetNextPoint();
                    if (isGameObjectMove) MoveGrid();
                    return;
                }
                StopMove();
                SetMovePosition(gridFinishPosition);
            }
        }
    }

    private void OnDestroy()
    {
        if(UnitManager.Instance != null)
            UnitManager.Instance.DeleteUnit(this);
    }

    public void OnSelectObject()
    {
        outline.selected = true;
    }

    public void OnDeselectObject()  {
        outline.selected = false;
    }

    public void OnSetTarget(TargetPoint point)
    {
        if(targetPoint != null)
            targetPoint.DeleteLink();
        if (!TerrainNavGrid.IsPositionCorrect(point.Position)) return;
        StopMove();
        targetPoint = point;
        targetPoint.AddLink();
        PathImage = new PathImage(GameParams.Width, GameParams.Length);
        SetMovePosition(targetPoint.Position);
    }

    public void SpawnObject(Vector2Int position)
    {
        gridPosition = position;
        this.gameObject.transform.position =
            new Vector3(position.x, TerrainHeightMap.Instance.GetHeight(position), position.y);
        TerrainNavGrid.Instance.Spawn(gridPosition);
    }

    public void SetMovePosition(Vector2Int position)
    {
        if (GameParams.GameMode == GameModes.Competitions && StepCount == GameConstants.MaxStepPoints) return;
        StopMove();
        timer.StopTimer();
        gridFinishPosition = position;
        if (TerrainNavGrid.Instance.IsCellUsed(gridFinishPosition) && 
            !PathFinder.FindFreeCell(gridFinishPosition, out gridFinishPosition, this.MaxHeight))
                return;
        TerrainNavGrid.Instance.FindPath(this, gridFinishPosition);
    }

    public void OnPathFound(List<Vector2Int> path)
    {
        movePath = path;
        attemptCount = GameConstants.MaxPathFindAttempt;
        timer.StopTimer();
    }

    private void MoveGrid()
    {
        if (GameParams.GameMode == GameModes.Competitions)
            StepCount++;
        TerrainNavGrid.Instance.MoveObject(gridPosition, gridNextPosition);
        gridPosition = gridNextPosition;
        PathImage += gridPosition;
        nextPosition = new Vector3(gridNextPosition.x, TerrainHeightMap.Instance.GetHeight(gridNextPosition), gridNextPosition.y);
    }

    private bool GetNextPoint()
    {
        if (!IsGetNextPoint()) return false;
        gridNextPosition = movePath[movePath.Count - 1];
        movePath.RemoveAt(movePath.Count - 1);
        return true;
    }

    private bool IsGetNextPoint()
    {
        return IsPathExists() && !TerrainNavGrid.Instance.IsCellUsed(movePath[movePath.Count - 1]);
    }

    private bool IsPathExists()
    {
        return movePath != null && movePath.Count != 0;
    }

    private void MoveGameObjectToPoint()
    {
        Vector3 position = this.gameObject.transform.position;
        Vector3 temp = nextPosition - position;
        this.gameObject.transform.Translate(new Vector3(temp.x * Time.deltaTime * speed, temp.y * Time.deltaTime * speed,
            temp.z * Time.deltaTime * speed), Space.World);
    }

    private void StopMove()
    {
        isGameObjectMove = false;
        movePath = null;
    }

    public void ResetCount()
    {
        StepCount = 0;
    }

    public void OnPathNotFound(FinishNotFoundReasons reason)
    {
        if (attemptCount == 0 || Vector2Int.Distance(this.gridPosition, gridFinishPosition) <= GameConstants.StopMoveMinDistance) return;
        attemptCount--;
        timer.SetTimer(reason == FinishNotFoundReasons.Blocked ? GameConstants.PathFindBlockedTime : GameConstants.PathFindAttemptTime);
    }

    private void TimerElapsed()
    {
        SetMovePosition(gridFinishPosition);
    }
}
