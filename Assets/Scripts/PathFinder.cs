using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathFinder {
    public Thread FindThread;
    public bool IsComplete { get; private set; }
    public bool IsActual { get; set; }
    public bool IsPathFound { get; private set; }
    private List<Vector2Int> path = new List<Vector2Int>();
    private float maxHeight = 0;

    public delegate void OnPathFound(List<Vector2Int> result);
    OnPathFound onPathfound;
    private Vector2Int start, finish;

    private static readonly Vector2Int[] MoveArray = {
        new Vector2Int( -1, -1 ),
        new Vector2Int( 0, -1 ),
        new Vector2Int( 1, -1 ),
        new Vector2Int( 1, 0 ),
        new Vector2Int( 1, 1 ),
        new Vector2Int( 0, 1 ),
        new Vector2Int( -1, 1 ),
        new Vector2Int( -1, 0 ),
    };

    public PathFinder(Vector2Int _start, Vector2Int _finish, float _maxHeight = 0) {
        start = _start;
        finish = _finish;
        maxHeight = _maxHeight;
        IsPathFound = false;
    }

    public PathFinder(Vector2Int _start, Vector2Int _finish, float _maxHeight, OnPathFound function) : this(_start, _finish, _maxHeight) {
        onPathfound = function;
    }


    public void FindPath()
    {
        //if (TerrainNavGrid.Instance.IsCellUsed(finish) && !FindFreeCell(finish, out finish, maxHeight)) return;
        bool[,] scannedCells = new bool[GameParams.Width, GameParams.Length];
        PriorityQueue<NavGridPoint> queue = new PriorityQueue<NavGridPoint>();
        NavGridPoint point = new NavGridPoint(start, 0), movePoint;
        point.distance = GetDistance(start, finish);
        queue.Add(point);
        scannedCells[start.x, start.y] = true;
        while(queue.Count != 0)
        {
            if (FindThread != null && (FindThread.ThreadState & ThreadState.AbortRequested) != 0 
                || TerrainNavGrid.Instance.IsCellUsed(finish) || !IsActual) return;
            point = queue.GetMin();
            if (point.Position == finish)
            {
                IsPathFound = true;
                break;
            }
            foreach(Vector2Int move in MoveArray)
            {
                movePoint = new NavGridPoint(point.Position + move, point.order + 1);
                if (!IsPointInField(movePoint.Position) || TerrainNavGrid.Instance.IsCellUsed(movePoint.Position) 
                    || TerrainHeightMap.Instance.GetHeight(movePoint.Position) > 
                    TerrainHeightMap.Instance.GetHeight(point.Position) + maxHeight) continue;
                movePoint.oldPoint = point;
                movePoint.distance = GetDistance(movePoint.Position, finish);
                if (scannedCells[movePoint.Position.x, movePoint.Position.y] )
                {
                    if (queue.Contains(movePoint))
                    {
                        NavGridPoint remove = queue.Find(movePoint);
                        if (remove.Weight < movePoint.Weight) continue;
                        queue.Remove(remove);
                        queue.Add(movePoint);
                    }
                    continue;
                }
                scannedCells[movePoint.Position.x, movePoint.Position.y] = true;
                queue.Add(movePoint);
            }
        }
        if (IsPathFound)
        {
            while (point.oldPoint != null)
            {
                path.Add(point.Position);
                point = point.oldPoint;
            }
        }
        IsComplete = true;
        if (onPathfound != null && IsPathFound)
            onPathfound.BeginInvoke(path, null, null);
    }

    public static bool FindFreeCell(Vector2Int position, out Vector2Int outpos, float _maxHeight = 0)
    {
        PriorityQueue<NavGridPoint> queue = new PriorityQueue<NavGridPoint>();
        NavGridPoint temp = new NavGridPoint(position, 0);
        queue.Add(temp);
        while (queue.Peek().order < GameConstants.FreeCellMaxOrder)
        {
            temp = queue.GetMin();
            if(!TerrainNavGrid.Instance.IsCellUsed(temp.Position))
            {
                outpos = temp.Position;
                return true;
            }
            foreach(Vector2Int v in MoveArray)
            {
                NavGridPoint add = new NavGridPoint(temp.Position + v, temp.order + 1);
                if (!IsPointInField(temp.Position)
                    || (_maxHeight != 0 && TerrainHeightMap.Instance.GetHeight(temp.Position) - 
                    TerrainHeightMap.Instance.GetHeight(add.Position) >= _maxHeight )) continue;
                add.distance = GetDistance(position, add.Position);
                queue.Add(add);
            }
        }
        outpos = position;
        return false;
    }

    private static float GetDistance(Vector2Int start, Vector2Int finish)
    {
        return Vector2Int.Distance(start, finish);
    }

    public static bool IsPointInField(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < GameParams.Width && pos.y >= 0 && pos.y < GameParams.Length;
    }
}
