using UnityEngine;

/// <summary>
/// Интерфейс для объектов, которые спавнятся на навигационной сетке
/// </summary>
public interface INavGridSpawn {
    void SpawnObject(Vector2Int position);
}