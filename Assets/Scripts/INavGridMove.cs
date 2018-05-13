using System.Collections.Generic;
using UnityEngine;

public interface INavGridMove {
    float MaxHeight { get; }
    PathFinder PathFind { get; set; }
    Vector2Int GridPosition { get; }
    void SetMovePosition(Vector2Int position);
    void OnPathFound(List<Vector2Int> path);
}