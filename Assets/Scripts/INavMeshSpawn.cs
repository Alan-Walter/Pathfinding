using UnityEngine;

public interface INavMeshSpawn
{
    Vector3 Position { get; }
    void SpawnObject();
}