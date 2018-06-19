using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Статический класс игровых параметров
/// </summary>
public static class GameParams {

    public static GameModes GameMode;  //  игровой режим
    public static CameraStates CameraState;  //  состояние камеры
    public static MapTypes MapType;  //  тип камеры
    public static GamePlayState GamePlayState;  //  состояние игры
    public static float MapScale;  // масштаб карты высот
    public static float Seed;  //  смещение карты
    public static int MapId;  //  номер карты

    public static int Width;  //  ширина карты
    public static int Height;  //  высота карты
    public static int Length;  //  длина карты

    public static List<string> MapNames;  //  названия карт

    public static void GetMapsNames() {
        if (Directory.Exists(GameConstants.MapPath))
            MapNames = Directory.GetFiles(GameConstants.MapPath).Where(a => a.Contains(GameConstants.MapFileEtension)).ToList();
        else Directory.CreateDirectory(GameConstants.MapPath);
    }
}
