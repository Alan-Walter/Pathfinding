using UnityEngine;
using System.Collections;

public enum GamePlayState {
    Play,
    Menu,
    Spawn,
    SelectParams,
    SelectStart,
    SelectFinish,
    End
}

public enum GameModes {
    Sandbox,
    Competitions
}

public enum MapTypes {
    Generation,
    Labyrinth,
    DefaultMaps
}

public enum ButtonKeyStates {
    Normal,
    Checked
}

public enum CameraStates { 
    Normal,
    Freeze
}

public enum FinishNotFoundReasons {
    FinishUsed,
    PathNotFound
}