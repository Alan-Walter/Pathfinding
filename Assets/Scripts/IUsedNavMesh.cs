using UnityEngine;
using System.Collections;

public interface IUsedNavMesh {
    Vector3 Position { get; }
    float MaxHeight { get; }
    void SpawnObject();
    void SetMovePosition(Vector3 position);
}
