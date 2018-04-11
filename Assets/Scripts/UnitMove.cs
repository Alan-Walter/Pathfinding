using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UnitMove : MonoBehaviour, IUsedNavMesh
{
    float maxHeight = 1.0f;
    private bool isMoved = false;
    Vector3? nextPoint = null;
    List<Vector3> path;
    private float speed = 10.2f;
    public Vector3 Position
    {
        get
        {
            return new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y),
            Mathf.RoundToInt(gameObject.transform.position.z));
        }
    }

    public float MaxHeight
    {
        get
        {
            return maxHeight;
        }
    }

    public void SpawnObject()
    {
        TerrainNavMesh.Instance.Spawn(this);
    }

    private void MoveObject(Vector3 position)
    {
        TerrainNavMesh.Instance.MoveObject(this, position);
    }

    public void SetMovePosition(Vector3 position)
    {
        isMoved = false;
        path = TerrainNavMesh.Instance.GetMovePath(this, position);
        if (path.Count == 0) return;
        nextPoint = path[path.Count - 1];
        path.RemoveAt(path.Count - 1);
        isMoved = true;
    }

    void Awake() {
        
    }

    // Use this for initialization
    void Start () {
        SpawnObject();
    }
	
	// Update is called once per frame
	void Update () {
	    if(!isMoved)
        {
            if (path == null || path.Count == 0) return;
            nextPoint = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);
            isMoved = true;
        }
        else
        {
            if (nextPoint == null) return;
            Vector3 move = (Vector3)nextPoint;
            Vector3 position = this.Position;
            Vector3 temp = move - position;
            this.transform.Translate(new Vector3(temp.x * Time.deltaTime * speed, temp.y * Time.deltaTime * speed, 
                temp.z * Time.deltaTime * speed), Space.World);
            if (position.x == move.x && position.z == move.z)
                isMoved = false;
        }
	}
}
