using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Scenes.Server;
using TTG_Shared.Packets;

namespace TTG_Game.Scenes; 

public class LobbyScene : SubScene {

    private readonly bool _isOwnerHost;
    private readonly Dictionary<Guid, Player> _players;
    private readonly ushort _maxPlayers;

    private readonly Text _playersText;
    private readonly Button _quitButton;
    private readonly Button? _startButton;

    public LobbyScene(bool owner, Dictionary<Guid, Player> players, ushort maxPlayers) {
        this._isOwnerHost = owner;
        this._players = players;
        this._maxPlayers = maxPlayers;

        this._quitButton = new Button(TTGGame.Instance.TextureManager.Quit) {
            Static = true,
            Scale = new Vector2(.6f)
        };
        this._quitButton.Position = new Vector2(10f, TTGGame.Instance.GraphicManager.ScreenHeight - this._quitButton.Rectangle.Height - 10f);
        this._quitButton.Click += this.OnQuitClick;

        if (this._isOwnerHost) {
            this._startButton = new Button(TTGGame.Instance.TextureManager.Start) {
                Static = true,
                Centered = true,
                Scale = new Vector2(.6f),
                Disabled = true
            };
            this._startButton.Position = new Vector2(0f, TTGGame.Instance.GraphicManager.ScreenHeight / 2 - this._startButton.Rectangle.Height / 2 + 10f);
        }

        this._playersText = new Text($"Players {this._players.Count} / {this._maxPlayers}") {
            Static = true,
            LayerDepth = 1f
        };
        this._playersText.Position = (this._startButton?.Position ?? new Vector2(TTGGame.Instance.GraphicManager.ScreenWidth / 2 - this._playersText.Measures.X / 2, TTGGame.Instance.GraphicManager.ScreenHeight - this._playersText.Measures.Y)) - new Vector2(0f, (this._isOwnerHost ? this._startButton?.Rectangle.Height / 2 ?? 0 : 0) + 10f);
    }

    private void OnQuitClick(object? sender, EventArgs e) {
        TTGGame.Instance.Scene = new ServerScene();
        TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Udp, new LeaveRoomPacket());

        TTGGame.Instance.Entities.Clear();
        TTGGame.Instance.NearbyEntities.Clear();
    }

    public override void Update(GameTime gameTime) {
        this._playersText.String = $"Players {this._players.Count} / {this._maxPlayers}";

        this._quitButton.Update(gameTime);

        if (this._startButton != null) {
            this._startButton.Disabled = this._players.Count != this._maxPlayers;
            this._startButton.Update(gameTime);
        }

        foreach (var player in this._players.Values)
            player.Update(gameTime);

        this.Camera!.Follow(this._players.First().Value);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.OrangeRed);
        
        this._playersText.Draw(gameTime);
        this._quitButton.Draw(gameTime);
        this._startButton?.Draw(gameTime);

        foreach (var player in this._players.Values)
            player.Draw(gameTime);
    }

}