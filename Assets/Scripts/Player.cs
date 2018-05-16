using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public string Name { get; private set; }
    public Color Color { get; private set; }
	// Use this for initialization
	public Player () {
        PlayerController.Instance.AddPlayer(this);
        this.Name = string.Format("Player {0}", PlayerController.Instance.GetPlayerCount());
        this.Color = Random.ColorHSV();
	}

    ~Player()
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.RemovePlayer(this);
    }

    public override string ToString()
    {
        return Name;
    }
}
