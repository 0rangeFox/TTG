using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class MainScene : Scene {

    private readonly Texture2D _buttonTexture = TTGGame.Instance.Load<Texture2D>("Controls/Button");
    private readonly Button _playButton;
    private readonly Button _quitButton;

    public MainScene() {
        this._playButton = new Button(this._buttonTexture, TTGGame.Instance.FontManager.AmongUs) {
            Position = new Vector2(250, 200),
            Text = "Play"
        };

        this._playButton.Click += PlayButton_Click;

        this._quitButton = new Button(this._buttonTexture, TTGGame.Instance.FontManager.AmongUs) {
            Position = new Vector2(250, 250),
            Text = "Quit"
        };

        this._quitButton.Click += QuitButton_Click;
    }

    private void PlayButton_Click(object sender, EventArgs e) => TTGGame.Instance.Scene = new LobbyScene();
    private void QuitButton_Click(object sender, EventArgs e) => TTGGame.Instance.Game.Exit();

    public override void Update(GameTime gameTime) {
        this._playButton.Update(gameTime);
        this._quitButton.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
        this._playButton.Draw(spriteBatch, gameTime);
        this._quitButton.Draw(spriteBatch, gameTime);
    }

}