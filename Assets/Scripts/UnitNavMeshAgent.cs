using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UnitNavMeshAgent : MonoBehaviour, INavMeshSpawn, INavMeshMove
{
    float maxHeight = 1.0f;
    private bool isMoved = false;
    Vector3 nextPoint;
    Vector3 targetPoint;
    Vector3 oldPos;
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

    void Awake() {
        
    }

    // Use this for initialization
    void Start () {
        oldPos = Position;
        SpawnObject();
    }
	
	// Update is called once per frame
	void Update () {
	    if(isMoved)
        {
            Vector3 position = this.Position;
            if (position.x != nextPoint.x || position.z != nextPoint.z)
            {
                Vector3 temp = nextPoint - gameObject.transform.position;
                this.transform.Translate(new Vector3(temp.x * Time.deltaTime * speed, temp.y * Time.deltaTime * speed,
                    temp.z * Time.deltaTime * speed), Space.World);
            }
            else
            {
                if (position.x == targetPoint.x && position.z == targetPoint.z)
                {
                    isMoved = false;
                    return;
                }
                isMoved = GetNextPoint();
                if (!isMoved)
                {
                    GetMovePath();
                    isMoved = GetNextPoint();
                }
                if (isMoved)
                    MoveObject();
            }
        }
	}

    public void SpawnObject()
    {
        TerrainNavMesh.Instance.Spawn(oldPos);
    }

    private void MoveObject()
    {
        TerrainNavMesh.Instance.MoveObject(oldPos, nextPoint);
        oldPos = nextPoint;
    }

    public void SetMovePosition(Vector3 position)
    {
        isMoved = false;
        targetPoint = position;
        GetMovePath();
        isMoved = GetNextPoint();
        MoveObject();
    }

    private bool GetNextPoint()
    {
        if (path == null || path.Count == 0)
            return false;
        nextPoint = path[path.Count - 1];
        path.RemoveAt(path.Count - 1);
        if (TerrainNavMesh.Instance.IsCellUsed(nextPoint))
            return false;
        return true;
    }

    private void GetMovePath()
    {
        path = TerrainNavMesh.Instance.GetMovePath(this, targetPoint);
    }
}
