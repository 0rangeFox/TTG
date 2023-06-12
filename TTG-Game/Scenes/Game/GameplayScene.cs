using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class GameplayScene : SubScene {

    private readonly Dictionary<Guid, Player> _players;

    public GameplayScene(Dictionary<Guid, Player> players) {
        this._players = players;
    }

    public override void Update(GameTime gameTime) {
        foreach (var player in this._players.Values)
            player.Update(gameTime);

        this.Camera!.Follow(this._players.First(player => !player.Value.IsDead).Value);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.ForestGreen);

        foreach (var player in this._players.Values)
            player.Draw(gameTime);
    }

}