using System.Collections;

public static class GameConstants {
    public const float CameraMinHeight = 10;
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

    public const int FreeCellMaxOrder = 100;

    public const float MaxDistance = 10;
    public const float FlagRadius = 4;

    public const float PathFindMinHeight = 2;

    public const int MaxPlayers = 2;
    public const int MaxStepPoints = 10;

    public const float StopMoveMinDistance = 15;

    public const int MaxPathFindAttempt = 6;
    public const float PathFindAttemptTime = 1;
    public const float PathFindBlockedTime = 3;

    public const double FindMaxTime = 1500;
}
