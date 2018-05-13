using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class GameParams {

    public static GameModes GameMode = GameModes.Sandbox;
    public static MapTypes MapType = MapTypes.Generation;
    public static float MapScale = 1.0f;
    public static float Seed = 1.0f;
    public static int MapId = 0;

    public static int Width = GameConstants.MinWidth;
    public static int Height = GameConstants.MaxHeight;
    public static int Length = GameConstants.MinLength;

    public static bool IsDefaultMapsLoaded { get; private set; }

    public static List<string> MapNames;

    public static void GetMapsNames()
    {
        if (Directory.Exists(GameConstants.MapPath))
            MapNames = Directory.GetFiles(GameConstants.MapPath).Where(a => a.Contains(GameConstants.MapFileEtension)).ToList();
        else Directory.CreateDirectory(GameConstants.MapPath);
        IsDefaultMapsLoaded = true;
    }
}
