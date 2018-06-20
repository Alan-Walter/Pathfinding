using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за поиск пути. Реализация алгоритма А*
/// </summary>
public class PathFinder {
    public Thread FindThread;  //  поток поиска пути
    public bool IsComplete { get; private set; }  //  завершилась ли работа алгоритма
    public bool IsActual { get; set; }  //  актуален ли поиск пути
    public bool IsPathFound { get; private set; }  //  найден ли путь
    private List<Vector2Int> path = new List<Vector2Int>();  //  список координат навигационной сетки до цели
    private float maxHeight = 0;  //  максимальная высота, на которую может взбираться 

    public delegate void OnPathFound(List<Vector2Int> result);  //  делегат, если путь найден
    public delegate void OnPathNotFound(FinishNotFoundReasons reason);  //  делегат, если путь не найден
    OnPathFound onPathfound;
    OnPathNotFound onPathNotFound;

    private Vector2Int start, finish;  //  координаты старта и финиша

    private DateTime startTime;  //  время начала поиска пути
    
    public double FindTime {  //  время работы алгоритма
        get
        {
            return (DateTime.UtcNow - startTime).TotalMilliseconds;
        }
    }

    //  массив перемещения по навигационной сетке
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

    #region Конструкторы класса

    public PathFinder(Vector2Int _start, Vector2Int _finish, float _maxHeight = 0) {
        start = _start;  //  устанавливаются позиции старта и финиша, максимальная высота
        finish = _finish;
        maxHeight = _maxHeight;
        IsActual = true;
    }

    public PathFinder(IFindPath unit, Vector2Int _finish): this(unit.GridPosition, _finish, unit.MaxHeight) {
        onPathfound = unit.OnPathFound;
        onPathNotFound = unit.OnPathNotFound;
    }
    #endregion

    /// <summary>
    /// Функция поиска пути
    /// </summary>
    public void FindPath() {
        startTime = DateTime.UtcNow;  //  получение времени начала работы
        FinishNotFoundReasons notFoundReason = FinishNotFoundReasons.None;
        bool[,] scannedCells = new bool[GameParams.Width, GameParams.Length];  //  просмотренные ячейки
        PriorityQueue<NavGridPoint> queue = new PriorityQueue<NavGridPoint>();  //  очередь с приоритетом из навигационных точек
        NavGridPoint point = new NavGridPoint(start, 0), movePoint;
        point.distance = GetDistance(start, finish);  //  получение дистанции между стартом и финишом
        queue.Add(point);
        scannedCells[start.x, start.y] = true;
        //  пока очередь не пустая, поиск пути актуален и поток не должен быть завершён
        while(queue.Count != 0 && IsActual && (FindThread == null || (FindThread.ThreadState & ThreadState.AbortRequested) == 0))
        {
            //  если финиш занят, выходим из цикла
            if(TerrainNavGrid.Instance.IsCellUsed(finish))
            {
                notFoundReason = FinishNotFoundReasons.FinishUsed;
                break;
            }
            point = queue.GetMin();
            //  если текущая позиция равна финишу, то выходим из цикла
            if (point.Position == finish)
            {
                IsPathFound = true;
                break;
            }
            if(IsPositionBlocked(point.Position))
            {
                notFoundReason = FinishNotFoundReasons.Blocked;
                break;
            }
            //  находим соседние клетки, ближайшие к финишу
            foreach(Vector2Int move in MoveArray)
            {
                movePoint = new NavGridPoint(point.Position + move, point.order + 1);

                if (!IsPointInField(movePoint.Position) || TerrainNavGrid.Instance.IsCellUsed(movePoint.Position) 
                    || TerrainHeightMap.Instance.GetHeight(movePoint.Position) > 
                    TerrainHeightMap.Instance.GetHeight(point.Position) + maxHeight) continue;
                //  пропускает поинт, если он вне карты, занят или выше текущей позиции

                //float heightDist = TerrainHeightMap.Instance.GetHeight(movePoint.Position) - TerrainHeightMap.Instance.GetHeight(point.Position);
                //if (heightDist < 0) heightDist = 0;
                movePoint.oldPoint = point;
                movePoint.distance = GetDistance(movePoint.Position, finish)/* + heightDist*/;
                if (scannedCells[movePoint.Position.x, movePoint.Position.y] )  //  если позиция просмотрена
                {
                    if (queue.Contains(movePoint))  //  если в очереди уже есть такая позиция, то берётся ближайшая к финишу
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
                //  добавление поинта в очередь с приоритетом
            }
        }
        IsComplete = true;
        if (IsPathFound)  //  если путь найден, получаем список координат
        {
            while (point.oldPoint != null)
            {
                path.Add(point.Position);
                point = point.oldPoint;
            }
            if (onPathfound == null) return;
            onPathfound.BeginInvoke(path, null, null);
        }
        else
        {
            // иначе сообщаем причину
            if (notFoundReason == FinishNotFoundReasons.None)
                notFoundReason = FinishNotFoundReasons.PathNotFound;
            if (onPathNotFound == null) return;
            onPathNotFound.BeginInvoke(notFoundReason, null, null);
        }
    }

    /// <summary>
    /// Функция поиска ближайшей к финишу точки
    /// </summary>
    /// <param name="position">координаты финиша</param>
    /// <param name="outpos">выходные координаты</param>
    /// <param name="_maxHeight">максимальная высота</param>
    /// <returns>true - найдено, false - не найдено</returns>
    public static bool FindFreeCell(Vector2Int position, out Vector2Int outpos, float _maxHeight = 0) {
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
                if (!IsPointInField(add.Position)
                    || (_maxHeight != 0 && TerrainHeightMap.Instance.GetHeight(temp.Position) - 
                    TerrainHeightMap.Instance.GetHeight(add.Position) >= _maxHeight )) continue;
                add.distance = GetDistance(position, add.Position);
                queue.Add(add);
            }
        }
        outpos = position;
        return false;
    }

    private static float GetDistance(Vector2Int start, Vector2Int finish) {  //  получение дистации между двумя координатами
        return Vector2Int.Distance(start, finish);
    }

    public static bool IsPointInField(Vector2Int pos) {  //  проверка на принадлежность координаты карте
        return pos.x >= 0 && pos.x < GameParams.Width && pos.y >= 0 && pos.y < GameParams.Length;
    }

    public static bool IsPositionBlocked(Vector2Int position) {  //  метод проверки на заблокированность со всех сторон координаты
        foreach(var x in MoveArray)
        {
            Vector2Int temp = x + position;
            if (IsPointInField(temp) && !TerrainNavGrid.Instance.IsCellUsed(temp)) return false;
        }
        return true;
    }
}
