using UnityEngine;
using System.Collections;

public static class GameStates {
    public static GamePlayState gamePlayState = GamePlayState.Play;
}

public enum GamePlayState {
    Play,
    Pause
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