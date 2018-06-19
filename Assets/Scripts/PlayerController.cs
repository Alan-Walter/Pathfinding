using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за контроль игроков
/// </summary>
public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }
    private List<Player> playerList = new List<Player>();
    private int index = 0;

    public static Player ActivePlayer { get; private set; }

    void Awake() {
        Instance = this;
    }

    void Start() {
        if (GameParams.GameMode == GameModes.Sandbox)
            ActivePlayer = new Player();
        else
        {
            for (int i = 0; i < GameConstants.MaxPlayers; i++)
                new Player();
            ActivePlayer = playerList.FirstOrDefault();
        }
    }
    // добавление игрока
    public void AddPlayer(Player player) {
        playerList.Add(player);
    }
    //  Удаление игрока
    public void RemovePlayer(Player player) {
        playerList.Remove(player);
    }
    //  Получение числа игроков
    public int GetPlayerCount() {
        return playerList.Count;
    }
    //  Переключение активного игрока
    public void NextPlayer() {
        index = (index + 1) % playerList.Count;
        ActivePlayer = playerList[index];
        TipsControls.Instance.SetPlayerInfo();
        SpawnMenuControls.IsPlayerSpawnBuild = false;
        SpawnMenuControls.IsPlayerSpawnUnit = false;
        UnitManager.Instance.ResetUnitStepCount();
        SelectGameObject.Instance.ClearSelectObjects();
    }
}
