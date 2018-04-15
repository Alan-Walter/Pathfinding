using UnityEngine;

public interface INavGridSpawn
{
    Vector3 Position { get; }
    void SpawnObject();
}