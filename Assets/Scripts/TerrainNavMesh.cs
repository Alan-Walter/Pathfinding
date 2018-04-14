using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerrainNavMesh : MonoBehaviour {

    struct NavPosition : IComparable
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

    private const int MinDistance = 3; 
    private bool[,] usedCell;
    public static TerrainNavMesh Instance { get; private set; }
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


    public void Spawn(Vector3 pos)
    {
        usedCell[(int)(pos.x), (int)(pos.z)] = true;
    }

    public bool IsCellUsed(Vector3 position)
    {
        return usedCell[(int)(position.x), (int)(position.z)];
    }

    public void MoveObject(Vector3 oldPos, Vector3 newPosition)
    {
        usedCell[(int)(oldPos.x), (int)(oldPos.z)] = false;
        usedCell[(int)(newPosition.x), (int)(newPosition.z)] = true;
    }

    public List<Vector3> GetMovePath(INavMeshMove moveObject, Vector3 finishPosition)
    {
        bool find = false;
        int fX = (int)finishPosition.x, fZ = (int)finishPosition.z;
        List<Vector3> result = new List<Vector3>();
        PriorityQueue<NavPosition> priorityQueue = new PriorityQueue<NavPosition>();
        bool[,] scanned = new bool[TerrainHeightMap.Instance.Width, TerrainHeightMap.Instance.Length];
        NavPosition position = new NavPosition((int)moveObject.Position.x, (int)moveObject.Position.z, 0);
        position.weight = GetDistance(position.x, position.z, fX, fZ);
        priorityQueue.Add(position);
        scanned[position.x, position.z] = true;

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
                if (temp.x < 0 || temp.x >= TerrainHeightMap.Instance.Width || temp.z < 0 || temp.z >= TerrainHeightMap.Instance.Length 
                    || scanned[temp.x, temp.z]) continue;
                scanned[temp.x, temp.z] = true;
                float height = TerrainHeightMap.Instance.GetHeight(temp.x, temp.z);
                if (usedCell[temp.x, temp.z] || height > moveObject.Position.y + moveObject.MaxHeight)
                    continue;
                temp.weight = temp.order + GetDistance(temp.x, temp.z, fX, fZ);
                temp.oldPoint = position;
                priorityQueue.Add(temp);
            }
        }
        if(find)
        {
            while(position.oldPoint != null)
            {
                Vector3 res = new Vector3(position.x, TerrainHeightMap.Instance.GetHeight(position.x, position.z), position.z);
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
