using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за параметры игрока
/// </summary>
public class Player {
    public string Name { get; private set; }  //  имя игрока
    public Color Color { get; private set; }  //  цвет юнитов игрока
	// Use this for initialization
	public Player () {
        PlayerController.Instance.AddPlayer(this);  //  добавление игрока в контроллер игроков
        this.Name = string.Format("Player {0}", PlayerController.Instance.GetPlayerCount());
        this.Color = Random.ColorHSV();  //  получение цвета игрока
	}

    ~Player() {
        if (PlayerController.Instance != null)
            PlayerController.Instance.RemovePlayer(this);
    }

    public override string ToString() {
        return Name;
    }
}
