using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за получение изображения пути движения юнита
/// </summary>
public class PathImage {
    bool isFirst = false;
    Texture2D texture;

    Vector2Int last;

    public PathImage(int width, int length) {
        texture = new Texture2D(width, length);
    }

    public static PathImage operator +(PathImage image, Vector2Int pos) {
        image.Add(pos);
        return image;
    }

    public void Add(Vector2Int pos) {
        if (!isFirst)
        {
            texture.SetPixel(pos.x, pos.y, Color.green);
            isFirst = true;
        }
        else
        {
            texture.SetPixel(pos.x, pos.y, Color.yellow);
            texture.SetPixel(last.x, last.y, Color.blue);
        }
        last = pos;
        texture.Apply();
    }

    public Texture2D GetImage() {
        return texture;
    }
}
