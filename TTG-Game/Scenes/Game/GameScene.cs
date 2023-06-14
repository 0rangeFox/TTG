using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Game.Scenes; 

public class GameScene : Scene, INetworkScene {

    private Guid _id;
    private bool _isOwnerHost;

    private SoundEffectInstance _backgroundSoundInstance;
    private Camera? _camera;

    private readonly Dictionary<Guid, Player> _players;
    private SubScene _scene;

    public IEnumerable<Player> Players => this._players.Values;

    public GameScene(Guid id, ushort maxPlayers, Color color, bool owner = true) {
        this._id = id;
        this._isOwnerHost = owner;
        this._players = new Dictionary<Guid, Player>(maxPlayers) {
            { (Guid) TTGGame.Instance.NetworkManager.ID, new Player((Guid) TTGGame.Instance.NetworkManager.ID, TTGGame.Instance.Nickname, color, Vector2.Zero) }
        };

        this._backgroundSoundInstance = TTGGame.Instance.AudioManager.GetSound(Sound.SpaceShipBackground);
        this._backgroundSoundInstance.IsLooped = true;
        this._backgroundSoundInstance.Volume = .25f;

        TTGGame.Instance.AudioManager.ChangeBackgroundStatus(false);
        this._backgroundSoundInstance.Play();

        this._camera = this.Camera = new Camera();
        this._scene = new LobbyScene(this._isOwnerHost, this._players, maxPlayers) {
            Camera = this.Camera
        };
    }

    ~GameScene() {
        this._backgroundSoundInstance.Dispose();
    }

    public GameScene(JoinRoomResultPacket jrrp) : this((Guid) jrrp.ID, (ushort) jrrp.MaxPlayers, ((System.Drawing.Color) jrrp.Color).ToXna(), false) {
       TTGGame.Instance.NetworkManager.SendPacket(new RequestRoomPlayersPacket());
    }

    public void PacketReceivedCallback(Packet packet) {
        switch (packet) {
            case ConnectRoomPacket crp:
                if (this._players.ContainsKey(crp.ID)) break;
                this._players.Add(crp.ID, new Player(crp.ID, crp.Nickname, crp.Color.ToXna(), crp.Position, true));
                TTGGame.Instance.AudioManager.Play(Sound.JoinRoom);
                break;
            case DisconnectRoomPacket drp:
                this._players.Remove(drp.ID);
                break;
            case PlayerMovementPacket pmp:
                if (this._players.TryGetValue((Guid) pmp.ID, out var player))
                    player.UpdatePosition(pmp);
                break;
            case StartRoomPacket srp:
                this._players[(Guid) TTGGame.Instance.NetworkManager.ID].Role = srp.Role;
                this.Camera = null;
                this._scene = new RoleRevealScene(srp.Role);
                break;
            case ReadyRoomPacket:
                this._scene = new GameplayScene(this._players) {
                    Camera = this.Camera = this._camera
                };
                break;
            case ExecuteActionPacket eap:
                switch (eap.Action) {
                    case Actions.Kill:
                        if (this._players.TryGetValue((Guid) eap.To, out var victim))
                            victim.Kill();
                        break;
                }
                break;
        }
    }

    public override void Update(GameTime gameTime) => this._scene.Update(gameTime);

    public override void Draw(GameTime gameTime) => this._scene.Draw(gameTime);

}