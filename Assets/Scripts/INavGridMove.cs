using UnityEngine;

public interface INavGridMove {
    Vector3 Position { get; }
    float MaxHeight { get; }
    void SetMovePosition(Vector3 position);
}