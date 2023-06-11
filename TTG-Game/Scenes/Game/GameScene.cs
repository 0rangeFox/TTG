using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Game.Scenes; 

public class GameScene : Scene, INetworkScene {

    private Guid _id;
    private bool _isOwnerHost;

    private readonly Dictionary<Guid, Player> _players;
    private SubScene _scene;

    public GameScene(Guid id, ushort maxPlayers, Color color, bool owner = true) {
        this._id = id;
        this._isOwnerHost = owner;
        this._players = new Dictionary<Guid, Player>(maxPlayers) {
            { (Guid) TTGGame.Instance.NetworkManager.ID, new Player(TTGGame.Instance.Nickname, color, Vector2.Zero) }
        };

        this.Camera = new Camera();
        this._scene = new LobbyScene(this._isOwnerHost, this._players, maxPlayers) {
            Camera = this.Camera
        };
    }

    public GameScene(JoinRoomResultPacket jrrp) : this((Guid) jrrp.ID, (ushort) jrrp.MaxPlayers, ColorExtension.GetFromSystemColor((System.Drawing.Color) jrrp.Color), false) {
       TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Udp, new RequestRoomPlayersPacket()); 
    }

    public override void Update(GameTime gameTime) => this._scene.Update(gameTime);

    public override void Draw(GameTime gameTime) => this._scene.Draw(gameTime);

    public void PacketReceivedCallback(Packet packet) {
        switch (packet) {
            case ConnectRoomPacket crp:
                if (!this._players.ContainsKey(crp.ID))
                    TTGGame.Instance.RunOnMainThread(() => this._players.Add(crp.ID, new Player(crp.Nickname, ColorExtension.GetFromSystemColor(crp.Color), crp.Position, true)));
                break;
            case DisconnectRoomPacket drp:
                TTGGame.Instance.RunOnMainThread(() => this._players.Remove(drp.ID));
                break;
            case PlayerMovementPacket pmp:
                if (this._players.TryGetValue((Guid) pmp.ID, out var player))
                    player.UpdatePosition(pmp);
                break;
        }
    }

}