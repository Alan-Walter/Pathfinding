using UnityEngine;

public interface INavMeshMove {
    Vector3 Position { get; }
    float MaxHeight { get; }
    void SetMovePosition(Vector3 position);
}