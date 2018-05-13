using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class TerrainNavGrid : MonoBehaviour {

    private bool[,] usedCell;
    public static TerrainNavGrid Instance { get; private set; }

    private List<PathFinder> pathFindThreads = new List<PathFinder>();
    private Queue<PathFinder> unitPathQueue = new Queue<PathFinder>();

    // Use this for initialization

    private void Awake() {
        Instance = this;
        usedCell = new bool[GameParams.Length, GameParams.Width];
    }

    void FixedUpdate() {
        for(int i = 0; i < pathFindThreads.Count; i++)
        {
            if(pathFindThreads[i].IsComplete || !pathFindThreads[i].FindThread.IsAlive)
            {
                pathFindThreads.RemoveAt(i);
                i--;
            }
        }
        while (pathFindThreads.Count < GameConstants.MaxThreads && unitPathQueue.Count != 0)
        {
            PathFinder temp = unitPathQueue.Dequeue();
            temp.FindThread = new Thread(temp.FindPath);
            temp.FindThread.IsBackground = true;
            pathFindThreads.Add(temp);
            temp.FindThread.Start();
        }
    }

    public void Spawn(Vector2Int pos) {
        usedCell[pos.y, pos.x] = true;
    }

    public void FindPath(INavGridMove unit, Vector2Int finish) {
        if (!IsCellUsed(finish) || IsCellUsed(finish) && PathFinder.FindFreeCell(finish, out finish, unit.MaxHeight))
        {
            if(unit.PathFind != null && unit.PathFind.FindThread.IsAlive)
                unit.PathFind.FindThread.Abort();
            unit.PathFind = new PathFinder(unit.GridPosition, finish, unit.MaxHeight, unit.OnPathFound);
            unitPathQueue.Enqueue(unit.PathFind);
        }
    }

    public bool IsCellUsed(Vector2Int position)
    {
        return usedCell[position.y, position.x];
    }

    public void MoveObject(Vector2Int oldPos, Vector2Int newPos)
    {
        usedCell[oldPos.y, oldPos.x] = false;
        usedCell[newPos.y, newPos.x] = true;
    }
}
