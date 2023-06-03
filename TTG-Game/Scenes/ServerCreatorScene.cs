using System;
using Microsoft.Xna.Framework;
using TTG_Game.Controls;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class ServerCreatorScene : SubScene {

    private const ushort MinPlayers = 4;
    private const ushort MaxPlayers = 16;
    private const ushort MinTraitors = 1;

    private ushort _maxPlayers = MinPlayers;
    private ushort _maxTraitors = MinTraitors;

    private readonly Text _titleScene;

    private readonly Text _serverText;
    private readonly TextField _serverTextField;

    private readonly Text _maxPlayersText;
    private readonly Button _incrementMaxPlayersButton;
    private readonly Button _decrementMaxPlayersButton;

    private readonly Text _maxTraitorsText;
    private readonly Button _incrementMaxTraitorsButton;
    private readonly Button _decrementMaxTraitorsButton;

    private readonly Button _createServerButton;

    public ServerCreatorScene() {
        this._titleScene = new Text("Creating server") {
            Position = new Vector2((TTGGame.Instance.GraphicManager.ScreenWidth / 2) - 50f, 25f)
        };

        this._serverTextField = new TextField($"{TTGGame.Instance.Nickname}'s Server") {
            Centered = true,
            Position = new Vector2(100f, -100f)
        };

        this._serverTextField.Change += this.ServerName_Change;

        this._serverText = new Text("Server name:") {
            Position = this._serverTextField.Position - new Vector2(this._serverTextField.Rectangle.Width / 2 + 20f, -5f)
        };

        this._maxPlayersText = new Text($"Max Players: {this._maxPlayers}") {
            Position = this._serverText.Position - new Vector2(0f, -75f)
        };

        this._incrementMaxPlayersButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
            Position = this._maxPlayersText.Position + new Vector2(this._maxPlayersText.Measures.X + 20f, 15f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) (3 * Math.PI) / 2
        };

        this._incrementMaxPlayersButton.Click += this.IncrementMaxPlayers_Click;

        this._decrementMaxPlayersButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
            Position = this._incrementMaxPlayersButton.Position + new Vector2(this._incrementMaxPlayersButton.Rectangle.Width, 5f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) Math.PI / 2,
            Disabled = true
        };

        this._decrementMaxPlayersButton.Click += this.DecrementMaxPlayers_Click;

        this._maxTraitorsText = new Text($"Max Traitors: {this._maxTraitors}") {
            Position = this._maxPlayersText.Position - new Vector2(0f, -75f)
        };
        
        this._incrementMaxTraitorsButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
            Position = this._maxTraitorsText.Position + new Vector2(this._maxTraitorsText.Measures.X + 20f, 15f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) (3 * Math.PI) / 2
        };

        this._incrementMaxTraitorsButton.Click += this.IncrementMaxTraitors_Click;

        this._decrementMaxTraitorsButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
            Position = this._incrementMaxTraitorsButton.Position + new Vector2(this._incrementMaxTraitorsButton.Rectangle.Width, 5f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) Math.PI / 2,
            Disabled = true
        };

        this._decrementMaxTraitorsButton.Click += this.DecrementMaxTraitors_Click;

        this._createServerButton = new Button("Create server") {
            Centered = true,
            Position = new Vector2(0f, TTGGame.Instance.GraphicManager.ScreenHeight - 300f),
        };

        this._createServerButton.Click += this.CreateServer_Click;
    }

    private void CheckEverything() {
        this._createServerButton.Disabled =
            this._serverTextField.String.Length > 3 &&
            this._maxPlayers >= MinPlayers && this._maxPlayers <= MaxPlayers &&
            this._maxTraitors >= MinTraitors && this._maxTraitors <= this._maxPlayers - MinPlayers;
    }

    private void ServerName_Change(object? sender, EventArgs e) => this.CheckEverything();

    private void IncrementMaxPlayers_Click(object? sender, EventArgs e) {
        this._decrementMaxPlayersButton.Disabled = false;
        this._maxPlayersText.String = $"Max Players: {++this._maxPlayers}";

        if (this._maxPlayers + 1 > MaxPlayers)
            this._incrementMaxPlayersButton.Disabled = true;

        this.CheckEverything();
    }

    private void DecrementMaxPlayers_Click(object? sender, EventArgs e) {
        this._incrementMaxPlayersButton.Disabled = false;
        this._maxPlayersText.String = $"Max Players: {--this._maxPlayers}";

        if (this._maxPlayers - 1 < MinPlayers)
            this._decrementMaxPlayersButton.Disabled = true;

        this.CheckEverything();
    }

    private void IncrementMaxTraitors_Click(object? sender, EventArgs e) {
        this._decrementMaxTraitorsButton.Disabled = false;
        this._maxTraitorsText.String = $"Max Traitors: {++this._maxTraitors}";

        if (this._maxTraitors + 1 > this._maxPlayers - MinPlayers)
            this._incrementMaxTraitorsButton.Disabled = true;

        this.CheckEverything();
    }

    private void DecrementMaxTraitors_Click(object? sender, EventArgs e) {
        this._incrementMaxTraitorsButton.Disabled = false;
        this._maxTraitorsText.String = $"Max Traitors: {--this._maxTraitors}";

        if (this._maxTraitors - 1 < MinTraitors)
            this._decrementMaxTraitorsButton.Disabled = true;

        this.CheckEverything();
    }

    private void CreateServer_Click(object? sender, EventArgs e) => TTGGame.Instance.Scene = new LobbyScene();

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        this._incrementMaxPlayersButton.Update(gameTime);
        this._decrementMaxPlayersButton.Update(gameTime);
        this._incrementMaxTraitorsButton.Update(gameTime);
        this._decrementMaxTraitorsButton.Update(gameTime);

        this._createServerButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
        this._titleScene.Draw(gameTime);

        this._serverText.Draw(gameTime);
        this._serverTextField.Draw(gameTime);
        this._maxPlayersText.Draw(gameTime);
        this._incrementMaxPlayersButton.Draw(gameTime);
        this._decrementMaxPlayersButton.Draw(gameTime);
        this._maxTraitorsText.Draw(gameTime);
        this._incrementMaxTraitorsButton.Draw(gameTime);
        this._decrementMaxTraitorsButton.Draw(gameTime);

        this._createServerButton.Draw(gameTime);
    }

}