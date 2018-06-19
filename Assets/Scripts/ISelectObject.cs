using UnityEngine;
using System.Collections;

public interface ISelectObject {
    Vector2Int GridPosition { get; }
    void OnSelectObject();
    void OnDeselectObject();
    void OnSetTarget(TargetPoint point);
}
