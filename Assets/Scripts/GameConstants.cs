using System.Collections;

public static class GameConstants {
    public const float CameraMinHeight = 0;
    public const float CameraMaxHeight = 300;
    public const float BorderThickness = 1;

    public const int MaxWidth = 1024;
    public const int MinWidth = 64;

    public const int MaxLength = 1024;
    public const int MinLength = 64;

    public const int MinHeight = 64;
    public const int MaxHeight = 256;

    public const int MazeMaxWidth = 256;
    public const int MazeMaxLength = 256;

    public const string MapPath = "Maps/";
    public const string MapFileEtension = ".awmap";

    //public const bool IsMapEditorActive = true;
    public const bool IsMapEditorActive = false;

    public const string MapEditorOutputFileName = "test";

    public const int MaxThreads = 3;

    public const int FreeCellMaxOrder = 5;
}
