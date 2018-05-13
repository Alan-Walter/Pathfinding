using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class Unit : MonoBehaviour, ISelectObject, INavGridSpawn, INavGridMove {

    private cakeslice.Outline outline;

    private bool isMove = false;

    private Vector2Int gridPosition;

    private Vector2Int gridNextPosition;
    private Vector3 nextPosition;

    private Vector2Int gridFinishPosition;

    private List<Vector2Int> movePath;

    private float maxHeight = 2;
    private float speed = 10.2f;

    private TargetPoint targetPoint;

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
    }
	
	// Update is called once per frame
	void Update () {
        if (!isMove) return;
        Vector3 position = this.gameObject.transform.position;
        if (Mathf.RoundToInt(position.x) != Mathf.RoundToInt(nextPosition.x) || Mathf.RoundToInt(position.z) != Mathf.RoundToInt(nextPosition.z))
            MoveGameObjectToPoint();
        else
        {
            if (movePath.Count == 0)
            {
                isMove = false;
                return;
            }
            isMove = GetNextPoint();
            if (!isMove)
                SetMovePosition(gridFinishPosition);
            else
                MoveGrid();
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
        targetPoint = point;
        targetPoint.AddLink();
        SetMovePosition(targetPoint.Position);
        StopMove();
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
        gridFinishPosition = position;
        TerrainNavGrid.Instance.FindPath(this, position);
    }

    public void OnPathFound(List<Vector2Int> path)
    {
        movePath = path;
        isMove = GetNextPoint();
        if (isMove)
            MoveGrid();
    }

    private void MoveGrid()
    {
        TerrainNavGrid.Instance.MoveObject(gridPosition, gridNextPosition);
        gridPosition = gridNextPosition;
        nextPosition = new Vector3(gridNextPosition.x, TerrainHeightMap.Instance.GetHeight(gridNextPosition), gridNextPosition.y);
    }

    private bool GetNextPoint()
    {
        if (movePath == null || movePath.Count == 0)
            return false;
        gridNextPosition = movePath[movePath.Count - 1];
        movePath.RemoveAt(movePath.Count - 1);
        if (TerrainNavGrid.Instance.IsCellUsed(gridNextPosition))
            return false;
        return true;
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
        isMove = false;
    }
}
