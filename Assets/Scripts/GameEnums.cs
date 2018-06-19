using UnityEngine;
using System.Collections;
/// <summary>
/// Состояния игры
/// </summary>
public enum GamePlayState {
    Play,
    Menu,
    Spawn,
    SelectParams,
    SelectStart,
    SelectFinish,
    End
}
/// <summary>
/// Игровые режимы
/// </summary>
public enum GameModes {
    Sandbox,
    Competitions
}
/// <summary>
///  Типы карты
/// </summary>
public enum MapTypes {
    Generation,
    Labyrinth,
    DefaultMaps
}
/// <summary>
/// Состояния клавиш интерфейса
/// </summary>
public enum ButtonKeyStates {
    Normal,
    Checked
}
/// <summary>
/// Состояния камеры
/// </summary>
public enum CameraStates { 
    Normal,
    Freeze
}
/// <summary>
/// Причины, по которым путь не был найден
/// </summary>
public enum FinishNotFoundReasons {
    None,
    FinishUsed,
    PathNotFound,
    Blocked
}