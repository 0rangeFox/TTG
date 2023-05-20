﻿using Microsoft.Xna.Framework;
using TTG_Game.Managers;

namespace TTG_Game;

public class TTGGame : Game {

    public static GameManager Instance;

    public TTGGame() {
        Instance = new GameManager(this);
    }

    protected override void Initialize() {
        base.Initialize();
        Instance.Initialize();
    }

}