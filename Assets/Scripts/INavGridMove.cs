using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для объектов, которые перемещаются по навигационной сетке
/// </summary>
public interface INavGridMove {
    void SetMovePosition(Vector2Int position);
}