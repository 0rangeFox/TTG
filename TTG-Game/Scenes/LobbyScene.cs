using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TTG_Game.Controls;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class LobbyScene : Scene {

    private readonly List<Player> _players = new();
    private readonly Entity _deadBody = new(TTGGame.Instance.TextureManager.CharacterDead);

    public LobbyScene() {
        this.Camera = new Camera();
        this._players.Add(new Player("0rangeFox", Color.Orange));

        this._deadBody.Highlight = true;
    }

    public override void Update(GameTime gameTime) {
        foreach (var player in this._players)
            player.Update(gameTime);

        this.Camera!.Follow(this._players[0]);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.OrangeRed);

        this._deadBody.Draw(gameTime);

        foreach (var player in this._players)
            player.Draw(gameTime);
    }

}