using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class LobbyScene : Scene {

    private readonly List<Player> _players = new();
    private readonly Sprite _deadBody = new(TTGGame.Instance.TextureManager.CharacterDead);

    public LobbyScene() {
        this.Camera = new Camera();
        this._players.Add(new Player("0rangeFox", Color.Orange));
    }

    public override void Update(GameTime gameTime) {
        foreach (var player in this._players)
            player.Update(gameTime);

        this.Camera!.Follow(this._players[0]);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.OrangeRed);

        this._deadBody.Draw(spriteBatch, gameTime);

        foreach (var player in this._players)
            player.Draw(spriteBatch, gameTime);
    }

}