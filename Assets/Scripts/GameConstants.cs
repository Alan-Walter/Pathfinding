using System.Collections;
/// <summary>
/// Класс, отвечающий за игровые константы
/// </summary>
public static class GameConstants {
    #region Параметры камеры
    public const float CameraMinHeight = 10;  //  минимальная высота
    public const float CameraMaxHeight = 300;  //  максимальная высота
    #endregion

    public const float BorderThickness = 1;  //  толщина рамки выделения

    #region Параметры карты
    public const int MaxWidth = 1024;  //  максимальная ширина
    public const int MinWidth = 64;  //  минимальная ширина

    public const int MaxLength = 1024;  //  максимальная длина
    public const int MinLength = 64;  //  минимальная длина

    public const int MinHeight = 64;  //  минимальная высота
    public const int MaxHeight = 256;  //  максимальная высота

    public const int MazeMaxWidth = 256;  //  максимальная ширина лабиринта
    public const int MazeMaxLength = 256;  //  максимальная длина лабиринта
    #endregion

    #region Параметры для редактора карты
    public const string MapPath = "Maps/";
    public const string MapFileEtension = ".awmap";

    public const bool IsMapEditorActive = false;

    public const string MapEditorOutputFileName = "test";
    #endregion

    public const int MaxThreads = 3;  //  максимальное кол-во потоков для поиска путей

    public const int FreeCellMaxOrder = 10;  //  максимальная ближайшая дистанция от точки финиша

    public const float MaxDistance = 10;  //  дистанция между флагами старта и финиша
    public const float FlagRadius = 4;  //  радиус зоны флага

    public const float PathFindMinHeight = 2;  //  минимальная высота, на которую может забираться юнит

    public const int MaxPlayers = 2;  //  максимальное кол-во игроков
    public const int MaxStepPoints = 10;  //  максимальное число шагов в режиме соревнования

    public const float StopMoveMinDistance = 20;  //  дистанция от финиша, на которой можно не искать путь заново

    public const int MaxPathFindAttempt = 4;  //  число перезапусков поиска пути
    public const float PathFindAttemptTime = 1.5f;  //  время до вызова функции поиска пути
    public const float PathFindBlockedTime = 3;  //  время до вызова функции поиска пути при условии, что юнит заблокирован

    public const double FindMaxTime = 2000;  //  максимально время поиска пути, после чего поток "убивается"
}
