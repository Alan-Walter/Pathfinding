using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour, INavGridSpawn {
    private Vector2Int position;
    public void SpawnObject(Vector2Int _position)
    {
        position = _position;
        this.gameObject.transform.position = 
            new Vector3(position.x, TerrainHeightMap.Instance.GetHeight(position.x, position.y), position.y);
        TerrainNavGrid.Instance.Spawn(position);
    }

    // Use this for initialization
    void Start () {
        SpawnObject(new Vector2Int(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)));
    }
}
