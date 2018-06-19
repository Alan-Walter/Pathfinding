using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для объектов, которые ищут путь 
/// </summary>
public interface IFindPath {
    Vector2Int GridPosition { get; }  //  позиция на сетке
    float MaxHeight { get; }  //  максимальная высота, на которую могут подняться
    PathFinder PathFind { get; set; }  //  объект класса PathFind
    void OnPathFound(List<Vector2Int> path);  //  метод для делегата в случае успеха поиска пути
    void OnPathNotFound(FinishNotFoundReasons reason);  //  метод для делега в случае провала поиска пути
}
