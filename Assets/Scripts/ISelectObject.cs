using UnityEngine;
using System.Collections;

/// <summary>
/// Интерфейс для объектов, которые можно выделять
/// </summary>
public interface ISelectObject {
    Vector2Int GridPosition { get; }
    void OnSelectObject();
    void OnDeselectObject();
    void OnSetTarget(TargetPoint point);
}
