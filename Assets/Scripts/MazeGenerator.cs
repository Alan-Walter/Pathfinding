using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator {
    private int width, height;
    private long unvisitedCount;
    private bool[,] result;
    private bool[,] visited;
    private readonly Vector2Int[] MoveArray = {
        new Vector2Int(0, -2),
        new Vector2Int(0, 2),
        new Vector2Int(-2, 0),
        new Vector2Int(2, 0)
    };

    public MazeGenerator(int _width, int _height)
    {
        width = _width;
        height = _height;
        unvisitedCount = width * height;
    }

    public bool [,] GenerateMaze()
    {
        Stack<Vector2Int> positions = new Stack<Vector2Int>();
        
        CreateMaze();
        Vector2Int currentPosition = new Vector2Int(1, 1), nextPosition;
        SetVisitedPosition(currentPosition);
        while (unvisitedCount != 0)
        {
            List<Vector2Int> neighbors = GetNeighbors(currentPosition);
            if (neighbors.Count == 0)
            {
                if (positions.Count != 0)
                    currentPosition = positions.Pop();
                else
                {
                    currentPosition = GetUnvisitedPosition();
                    SetVisitedPosition(currentPosition);
                }
                continue;
            }
            positions.Push(currentPosition);
            nextPosition = neighbors[Random.Range(0, neighbors.Count)];
            RemoveWall(currentPosition, nextPosition);
            currentPosition = nextPosition;
            SetVisitedPosition(currentPosition);
        }
        return result;
    }

    private void CreateMaze()
    {
        result = new bool[height, width];
        visited = new bool[height, width];
        for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
                if (i % 2 == 0 || j % 2 == 0 || i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    result[j, i] = true;
                    SetVisitedPosition(new Vector2Int(i, j));
                }
    }

    private void RemoveWall(Vector2Int first, Vector2Int second)
    {
        Vector2Int diff = second - first;
        diff.x = diff.x != 0 ? diff.x / Mathf.Abs(diff.x) : 0;
        diff.y = diff.y != 0 ? diff.y / Mathf.Abs(diff.y) : 0;
        result[first.y + diff.y, first.x + diff.x] = false;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach(Vector2Int x in MoveArray)
        {
            Vector2Int temp = x + pos;
            if (temp.x < 0 || temp.x >= width || temp.y < 0 || temp.y >= height || visited[temp.y, temp.x]) continue;
            result.Add(temp);
        }
        return result;
    }

    private Vector2Int GetUnvisitedPosition()
    {
        for (int j = height - 2; j > 0; j--)
            for (int i = width - 2; i > 0; i--)
                if (!visited[j, i]) return new Vector2Int(i, j);
        return new Vector2Int();
    }

    private void SetVisitedPosition(Vector2Int position)
    {
        visited[position.y, position.x] = true;
        unvisitedCount--;
    }
}
