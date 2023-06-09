﻿using Microsoft.Xna.Framework;
using TTG_Game.Managers;

namespace TTG_Game;

public class TTGGame : Game {

    public static GameManager Instance;

    public TTGGame() => Instance = new GameManager(this);

    protected override void Initialize() => Instance.Initialize();

    protected override void Update(GameTime gameTime) => Instance.Update(gameTime);

    protected override void Draw(GameTime gameTime) => Instance.Draw(gameTime);

}