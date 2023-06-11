using System;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Models.Graphics;
using TTG_Game.Utils;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Game.Scenes; 

public class ServerCreatorScene : SubScene, INetworkScene {

    private const ushort MinPlayers = 4;
    private const ushort MaxPlayers = 16;
    private const ushort MinTraitors = 1;

    private ushort _players = MinPlayers;
    private ushort _traitors = MinTraitors;
    private int _minPlayers = MinPlayers;
    private int _maxTraitors = MinTraitors;

    private readonly Text _titleScene;

    private readonly Text _serverText;
    private readonly TextField _serverTextField;

    private readonly Text _playersText;
    private readonly Button _incrementPlayersButton;
    private readonly Button _decrementPlayersButton;

    private readonly Text _maxTraitorsText;
    private readonly Button _incrementTraitorsButton;
    private readonly Button _decrementTraitorsButton;

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

        this._playersText = new Text($"Max Players: {this._players}") {
            Position = this._serverText.Position - new Vector2(0f, -75f)
        };

        this._incrementPlayersButton = new Button(TTGGame.Instance.TextureManager.GetTexture(Texture.ArrowRight)) {
            Position = this._playersText.Position + new Vector2(this._playersText.Measures.X + 20f, 15f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) ConverterUtil.DegreesToRadians(270)
        };

        this._incrementPlayersButton.Click += this.IncrementPlayersClick;

        this._decrementPlayersButton = new Button(TTGGame.Instance.TextureManager.GetTexture(Texture.ArrowRight)) {
            Position = this._incrementPlayersButton.Position + new Vector2(this._incrementPlayersButton.Rectangle.Width, 5f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) ConverterUtil.DegreesToRadians(90),
            Disabled = true
        };

        this._decrementPlayersButton.Click += this.DecrementPlayersClick;

        this._maxTraitorsText = new Text($"Max Traitors: {this._traitors}") {
            Position = this._playersText.Position - new Vector2(0f, -75f)
        };
        
        this._incrementTraitorsButton = new Button(TTGGame.Instance.TextureManager.GetTexture(Texture.ArrowRight)) {
            Position = this._maxTraitorsText.Position + new Vector2(this._maxTraitorsText.Measures.X + 20f, 15f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) ConverterUtil.DegreesToRadians(270),
            Disabled = true
        };

        this._incrementTraitorsButton.Click += this.IncrementTraitorsClick;

        this._decrementTraitorsButton = new Button(TTGGame.Instance.TextureManager.GetTexture(Texture.ArrowRight)) {
            Position = this._incrementTraitorsButton.Position + new Vector2(this._incrementTraitorsButton.Rectangle.Width, 5f),
            Scale = new Vector2(.25f, .85f),
            Rotation = (float) ConverterUtil.DegreesToRadians(90),
            Disabled = true
        };

        this._decrementTraitorsButton.Click += this.DecrementTraitorsClick;

        this._createServerButton = new Button("Create server") {
            Centered = true,
            Position = new Vector2(0f, TTGGame.Instance.GraphicManager.ScreenHeight - 300f),
        };

        this._createServerButton.Click += this.CreateServer_Click;
    }

    private void CheckEverything() {
        this._createServerButton.Disabled = !(
            this._serverTextField.String.Length > 3 &&
            this._players >= this._minPlayers && this._players <= MaxPlayers &&
            this._traitors >= MinTraitors && this._traitors <= this._maxTraitors
        );
    }

    private void ServerName_Change(object? sender, EventArgs e) => this.CheckEverything();

    private void CheckPlayersAndTraitorsValues() {
        this._minPlayers = 4 * this._traitors + 1 - 4;
        this._maxTraitors = (this._players - 1) / 4 + 1;

        this._incrementPlayersButton.Disabled = this._players + 1 > MaxPlayers;
        this._decrementPlayersButton.Disabled = this._players <= this._minPlayers;
        this._incrementTraitorsButton.Disabled = this._traitors + 1 > this._maxTraitors;
        this._decrementTraitorsButton.Disabled = this._traitors - 1 < MinTraitors;

        this.CheckEverything();
    }

    private void IncrementPlayersClick(object? sender, EventArgs e) {
        this._playersText.String = $"Max Players: {++this._players}";
        this.CheckPlayersAndTraitorsValues();
    }

    private void DecrementPlayersClick(object? sender, EventArgs e) {
        this._playersText.String = $"Max Players: {--this._players}";
        this.CheckPlayersAndTraitorsValues();
    }

    private void IncrementTraitorsClick(object? sender, EventArgs e) {
        this._maxTraitorsText.String = $"Max Traitors: {++this._traitors}";
        this.CheckPlayersAndTraitorsValues();
    }

    private void DecrementTraitorsClick(object? sender, EventArgs e) {
        this._maxTraitorsText.String = $"Max Traitors: {--this._traitors}";
        this.CheckPlayersAndTraitorsValues();
    }

    private void ChangeStatusOfActions(bool status) {
        this.BackButton.Disabled =
            this._serverTextField.Disabled =
                this._incrementPlayersButton.Disabled =
                    this._decrementPlayersButton.Disabled =
                        this._incrementTraitorsButton.Disabled =
                            this._decrementTraitorsButton.Disabled =
                                this._createServerButton.Disabled = status;
    }

    private void CreateServer_Click(object? sender, EventArgs e) {
        this.ChangeStatusOfActions(true);

        TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Tcp, new CreateRoomPacket(TTGGame.Instance.Nickname, this._serverTextField.String, this._players, this._traitors));
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        this._serverTextField.Update(gameTime);
        this._incrementPlayersButton.Update(gameTime);
        this._decrementPlayersButton.Update(gameTime);
        this._incrementTraitorsButton.Update(gameTime);
        this._decrementTraitorsButton.Update(gameTime);

        this._createServerButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
        this._titleScene.Draw(gameTime);

        this._serverText.Draw(gameTime);
        this._serverTextField.Draw(gameTime);
        this._playersText.Draw(gameTime);
        this._incrementPlayersButton.Draw(gameTime);
        this._decrementPlayersButton.Draw(gameTime);
        this._maxTraitorsText.Draw(gameTime);
        this._incrementTraitorsButton.Draw(gameTime);
        this._decrementTraitorsButton.Draw(gameTime);

        this._createServerButton.Draw(gameTime);
    }

    public void PacketReceivedCallback(Packet packet) {
        if (packet is not CreatedRoomPacket { Created: true } crp) return;
        TTGGame.Instance.RunOnMainThread(() => TTGGame.Instance.Scene = new GameScene((Guid) crp.ID, this._players, ColorExtension.GetFromSystemColor((System.Drawing.Color) crp.Color)));
    }

}