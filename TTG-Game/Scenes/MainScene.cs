using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace TTG_Game.Scenes; 

public class MainScene : Scene {

    private readonly Texture2D _logo = TTGGame.Instance.Load<Texture2D>("Images/Logo");
    private readonly Texture2D _background = TTGGame.Instance.Load<Texture2D>("Images/Background");
    private readonly Text _nicknameText;
    private readonly TextField _nicknameTextField;
    private readonly Button _playButton;
    private readonly Button _quitButton;

    private void Nickname_Change(object? sender, EventArgs e) => TTGGame.Instance.Nickname = this._nicknameTextField.String;
    private static void PlayButton_Click(object? sender, EventArgs e) => TTGGame.Instance.Scene = new LobbyScene();
    private static void QuitButton_Click(object? sender, EventArgs e) => TTGGame.Instance.Game.Exit();

    public MainScene() {
        this._nicknameText = new Text("Nickname") {
            Position = new Vector2(5f, TTGGame.Instance.GraphicManager.ScreenHeight - 45f)
        };

        this._nicknameTextField = new TextField(TTGGame.Instance.Nickname) {
            Position = new Vector2(this._nicknameText.Measures.X + 10f, TTGGame.Instance.GraphicManager.ScreenHeight - TTGGame.Instance.TextureManager.TextField.Height - 5f)
        };

        this._nicknameTextField.Change += Nickname_Change;

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
        this._nicknameTextField.Update(gameTime);
        this._playButton.Update(gameTime);
        this._quitButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        var spriteBatch = TTGGame.Instance.SpriteBatch;
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        spriteBatch.Draw(this._background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(.105f, .12f), SpriteEffects.None, 0f);
        spriteBatch.DrawCenter(this._logo, new Vector2(0f, -125f), Color.White);

        this._nicknameText.Draw(gameTime);
        this._nicknameTextField.Draw(gameTime);
        this._playButton.Draw(gameTime);
        this._quitButton.Draw(gameTime);
    }

}