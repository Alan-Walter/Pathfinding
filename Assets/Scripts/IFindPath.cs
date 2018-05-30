using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFindPath {
    Vector2Int GridPosition { get; }
    float MaxHeight { get; }
    PathFinder PathFind { get; set; }
    void OnPathFound(List<Vector2Int> path);
    void OnPathNotFound(FinishNotFoundReasons reason);
}
