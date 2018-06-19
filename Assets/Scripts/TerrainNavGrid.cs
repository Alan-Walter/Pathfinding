using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

/// <summary>
/// Класс навигационной сетки террейна
/// </summary>
public class TerrainNavGrid : MonoBehaviour {

    private bool[,] usedCell;  //  навигационная сетка
    public static TerrainNavGrid Instance { get; private set; }

    private List<PathFinder> pathFindThreads = new List<PathFinder>();  //  список активных потоков поиска пути
    private Queue<PathFinder> unitPathQueue = new Queue<PathFinder>();  //  очередь из поиска путей

    // Use this for initialization

    private void Awake() {
        Instance = this;
        usedCell = new bool[GameParams.Length, GameParams.Width];
    }

    void FixedUpdate() {
        for (int i = 0; i < pathFindThreads.Count; i++)  //  проверка всех потоков
        {
            //  если поток работает больше максимального времени или не актуален, то он завершается
            if(pathFindThreads[i].FindThread.IsAlive && (pathFindThreads[i].FindTime > GameConstants.FindMaxTime || !pathFindThreads[i].IsActual))
                pathFindThreads[i].FindThread.Abort();
            else
            //  если поток завершился или работа алгоритма завершена, то поток удаляется из списка
            if (!pathFindThreads[i].FindThread.IsAlive || pathFindThreads[i].IsComplete)
            {
                pathFindThreads.RemoveAt(i);
                i--;
            }
        }
        //  пока очередь не пустая и кол-во потоков меньше максимального, то создаются новые потоки
        while (pathFindThreads.Count < GameConstants.MaxThreads && unitPathQueue.Count != 0)
        {
            PathFinder temp = unitPathQueue.Dequeue();
            if (temp == null)
                return;
            if (!temp.IsActual) continue;
            temp.FindThread = new Thread(temp.FindPath);
            temp.FindThread.IsBackground = true;
            pathFindThreads.Add(temp);
            temp.FindThread.Start();
        }
    }

    //  спавн объекта на навигационной сетке
    public void Spawn(Vector2Int pos) {
        if (!IsPositionCorrect(pos)) return;
        usedCell[pos.y, pos.x] = true;
    }

    //  поиск пути для объекта
    public void FindPath(IFindPath unit, Vector2Int finish) {
        if (IsCellUsed(finish)) return;
        if (unit.PathFind != null)
        {
            if(unit.PathFind.FindThread != null && unit.PathFind.FindThread.IsAlive)
                unit.PathFind.FindThread.Abort();
            unit.PathFind.IsActual = false;
        }
        unit.PathFind = new PathFinder(unit, finish);
        unitPathQueue.Enqueue(unit.PathFind);
    }

    //  проверка на занятость ячейки
    public bool IsCellUsed(Vector2Int position) {
        if (!IsPositionCorrect(position)) return true;
        return usedCell[position.y, position.x];
    }

    //  перемещение объекта
    public void MoveObject(Vector2Int oldPos, Vector2Int newPos) {
        usedCell[oldPos.y, oldPos.x] = false;
        usedCell[newPos.y, newPos.x] = true;
    }

    //  проверка на корректность координат
    public static bool IsPositionCorrect(Vector2Int pos) {
        return !(pos.x < 0 || pos.x >= GameParams.Width || pos.y < 0 || pos.y >= GameParams.Length);
    }

    void OnGUI() {
        GUI.Label(new Rect(0, Screen.height - 40, 150, 40), string.Format("Threads count: {0}\nQueue count: {1}", 
            pathFindThreads.Count, unitPathQueue.Count));
    }
}
