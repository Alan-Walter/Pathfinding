using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathImage {
    bool[,] field;
    private int width, length;
    bool isFirst = false;

    Vector2Int start, finish, last;

    public PathImage(int width, int length)
    {
        this.width = width;
        this.length = length;
        field = new bool[length, width];
    }

    public static PathImage operator +(PathImage image, Vector2Int pos)
    {
        image.Add(pos);
        return image;
    }

    public void Add(Vector2Int pos)
    {
        if (!isFirst)
        {
            start = pos;
            isFirst = true;
        }
        last = pos;
        field[pos.y, pos.x] = true;
    }

    public void AddFinish(Vector2Int pos)
    {
        finish = pos;
    }

    public Texture2D CreateImage()
    {
        Texture2D result = new Texture2D(width, length);
        for (int i = 0; i < length; i++)
            for (int j = 0; j < width; j++)
                if (field[i, j])
                    result.SetPixel(j, i, Color.blue);
        result.SetPixel(start.x, start.y, Color.green);
        result.SetPixel(last.x, last.y, Color.yellow);
        result.SetPixel(finish.x, finish.y, Color.red);
        result.Apply();
        return result;
    }
}
