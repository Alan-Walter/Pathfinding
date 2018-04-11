using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct NavPosition : IComparable
{
    public int x, z, order;
    public float weight;
    public object oldPoint;

    public int CompareTo(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) return -1;
        NavPosition b = (NavPosition)obj;
        if (this.weight < b.weight) return 1;
        else if (this.weight == b.weight) return 0;
        return -1;
    }
    
    public NavPosition(int x, int z, int order)
    {
        this.x = x;
        this.z = z;
        this.order = order;
        weight = 0;
        oldPoint = null;
    }

    public override bool Equals(object obj)
    {
        NavPosition temp = (NavPosition)obj;
        if (this.x == temp.x && this.z == temp.z) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

};

public class TerrainNavMesh : MonoBehaviour {
    
    private const int MinDistance = 3; 
    private bool[,] usedCell;
    public static TerrainNavMesh Instance { get; private set; }
    private List<IUsedNavMesh> usedCellList = new List<IUsedNavMesh>();
    private sbyte[,] moveArray = {
        { -1, -1 },
        { 0, -1 },
        { 1, -1 },
        { 1, 0 },
        { 1, 1 },
        { 0, 1 },
        { -1, 1 },
        { -1, 0 },
    };

    // Use this for initialization

    private void Awake()
    {
        Instance = this;
        usedCell = new bool[TerrainHeightMap.Instance.Width, TerrainHeightMap.Instance.Length];
    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Spawn(IUsedNavMesh spawnObject)
    {
        usedCell[(int)(spawnObject.Position.x), (int)(spawnObject.Position.z)] = true;
        usedCellList.Add(spawnObject);
    }

    public bool IsCellUsed(Vector3 position)
    {
        return usedCell[(int)(position.x), (int)(position.z)];
    }

    public void MoveObject(IUsedNavMesh moveObject, Vector3 newPosition)
    {
        usedCell[(int)(moveObject.Position.x), (int)(moveObject.Position.z)] = false;
        usedCell[(int)(newPosition.x), (int)(newPosition.z)] = true;
    }

    public List<Vector3> GetMovePath(IUsedNavMesh moveObject, Vector3 finishPosition)
    {
        bool[,] scannedCells = new bool[TerrainHeightMap.Instance.Width, TerrainHeightMap.Instance.Length];
        int fX = (int)finishPosition.x, fZ = (int)finishPosition.z;
        List<Vector3> result = new List<Vector3>();
        PriorityQueue<NavPosition> priorityQueue = new PriorityQueue<NavPosition>();
        Hashtable table = new Hashtable();

        NavPosition position = new NavPosition((int)moveObject.Position.x, (int)moveObject.Position.z, 0);
        position.weight = GetDistance(position.x, position.z, fX, fZ);
        priorityQueue.Add(position);
        scannedCells[position.x, position.z] = true;
        bool find = false;

        float[,] heightMap = TerrainHeightMap.Instance.HeightMap;

        while(priorityQueue.Count != 0)
        {
            position = priorityQueue.GetMin();
            if (position.x == fX && position.z == fZ)
            {
                find = true;
                break;
            }

            for(int i = 0; i < moveArray.GetLength(0); i++)
            {
                NavPosition temp = new NavPosition(position.x - moveArray[i, 0], position.z - moveArray[i, 1], position.order + 1);
                if (temp.x < 0 || temp.x >= TerrainHeightMap.Instance.Width || temp.z < 0 || temp.z >= TerrainHeightMap.Instance.Length) continue;
                scannedCells[temp.x, temp.z] = true;
                if (usedCell[temp.x, temp.z] || heightMap[temp.x, temp.z] > moveObject.Position.y + moveObject.MaxHeight) continue;
                temp.weight = temp.order + GetDistance(temp.x, temp.z, fX, fZ);
                temp.oldPoint = position;
                priorityQueue.Add(temp);
            }

            if (table.ContainsKey(position))
            {
                NavPosition temp = (NavPosition)table[position];
                if (temp.weight < position.weight)
                {
                    table.Remove(position);
                    table.Add(temp, null);
                }
            }
            else
            table.Add(position, null);
        }
        if(find)
        {
            while(position.oldPoint != null)
            {
                Vector3 res = new Vector3(position.x, heightMap[position.x, position.z] * TerrainHeightMap.Instance.Height, position.z);
                position = (NavPosition)position.oldPoint;
                result.Add(res);
            }
        }
        return result;
    }

    private float GetDistance(int x1, int z1, int x2, int z2)
    {
        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(z2 - z1, 2));
    }
}
