using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class GameParams {

    public static GameModes GameMode;
    public static CameraStates CameraState;
    public static MapTypes MapType;
    public static GamePlayState GamePlayState;
    public static float MapScale;
    public static float Seed;
    public static int MapId;

    public static int Width;
    public static int Height;
    public static int Length;

    public static List<string> MapNames;

    public static void GetMapsNames()
    {
        if (Directory.Exists(GameConstants.MapPath))
            MapNames = Directory.GetFiles(GameConstants.MapPath).Where(a => a.Contains(GameConstants.MapFileEtension)).ToList();
        else Directory.CreateDirectory(GameConstants.MapPath);
    }
}
