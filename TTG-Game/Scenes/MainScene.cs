using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;

namespace TTG_Game.Scenes; 

public class MainScene : Scene {

    private readonly Texture2D _logo = TTGGame.Instance.Load<Texture2D>("Images/Logo");
    private readonly Texture2D _background = TTGGame.Instance.Load<Texture2D>("Images/Background");
    private readonly Button _playButton;
    private readonly Button _quitButton;

    private static void PlayButton_Click(object? sender, EventArgs e) => TTGGame.Instance.Scene = new ServerSelectorScene();
    private static void QuitButton_Click(object? sender, EventArgs e) => TTGGame.Instance.Game.Exit();

    public MainScene() {
        this._playButton = new Button("Play") {
            Centered = true,
            Position = new Vector2(0f, 30f)
        };

        this._playButton.Click += PlayButton_Click;

        this._quitButton = new Button("Quit") {
            Centered = true,
            Position = new Vector2(0, 120f)
        };

        this._quitButton.Click += QuitButton_Click;
    }

    public override void Update(GameTime gameTime) {
        this._playButton.Update(gameTime);
        this._quitButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        var spriteBatch = TTGGame.Instance.SpriteBatch;
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        spriteBatch.Draw(this._background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(.105f, .12f), SpriteEffects.None, 0f);
        spriteBatch.DrawCenter(this._logo, new Vector2(0f, -125f), Color.White);

        this._playButton.Draw(gameTime);
        this._quitButton.Draw(gameTime);
    }

}