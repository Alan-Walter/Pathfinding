using UnityEngine;
using System.Collections;

public class GuiRectangle {
    private Texture2D texture;

    public GuiRectangle(int width, int height)
    {
        texture = new Texture2D(width, height);
    }

    public void SetColor(Color color)
    {
        for (int j = 0; j < texture.height; j++)
            for (int i = 0; i < texture.width; i++)
                texture.SetPixel(i, j, color);
        texture.Apply();
    }

    public void DrawBorderTexture(Rect rect, Color color, float thickness)
    {
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, thickness), texture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, thickness, rect.height), texture);
        GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), texture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), texture);
    }
}
