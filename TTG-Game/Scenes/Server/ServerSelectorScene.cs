using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Game.Scenes;

public class ServerSelectorScene : SubScene, INetworkScene {

    private class ServerData : ServerRowData {

        public float Height;
        public Text Title;
        public Button JoinButton;

        public ServerData(ServerRowData server, float height) : base(server) {
            this.Height = height;

            var serverText = this.Text.Length > 25 ? this.Text[..25] + "..." : this.Text;
            var text = $"{serverText} | Max Traitors: {this.MaxTraitors} | Players: {this.Players} / {this.MaxPlayers}".ToLower();
            var textMeasures = TTGGame.Instance.FontManager.AmongUs24px.MeasureString(text);

            this.Title = new Text(text) {
                Origin = new Vector2(textMeasures.X / 2f, textMeasures.Y / 2f),
                Position = TTGGame.Instance.GraphicManager.ScreenCenter + new Vector2(-300f + textMeasures.X / 2, this.Height)
            };

            var test = TTGGame.Instance.TextureManager.ArrowRight;
            this.JoinButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
                Centered = true,
                Position = new Vector2(300f, this.Height + 13f),
                Scale = new Vector2(.45f, .55f)
            };
        }

    }

    private class ServerDictionary {

        private readonly Dictionary<Guid, ServerData> _dictionary = new();
        private readonly Action<bool> _changeStatusOfActions;

        public Dictionary<Guid, ServerData>.ValueCollection Values => this._dictionary.Values;

        public ServerDictionary(Action<bool> changeStatusOfActions) {
            this._changeStatusOfActions = changeStatusOfActions;
        }

        public void Add(ServerRowData server) {
            var serverData = new ServerData(server, this._dictionary.Values.Count > 0 ? this._dictionary.Values.Last().Height + 55 : -125);
            serverData.JoinButton.Click += (_, _) => {
                this._changeStatusOfActions(true);
                TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Tcp, new JoinRoomPacket(serverData.ID, TTGGame.Instance.Nickname));
            };
            this._dictionary.Add(server.ID, serverData);
        }

        public bool Remove(Guid key) {
            var result = this._dictionary.Remove(key);

            if (result)
                ReorderValuesByHeight();

            return result;
        }

        private void ReorderValuesByHeight() {
            float height = -125;
            foreach (var server in this.Values) {
                server.Height = height;
                height += 55;
            }
        }

    }

    private readonly Text _titleScene;
    private readonly Button _createServerButton;
    private readonly Text _nicknameText;
    private readonly TextField _nicknameTextField;

    private readonly ServerDictionary _servers;

    private void Nickname_Change(object? sender, EventArgs e) => TTGGame.Instance.Nickname = this._nicknameTextField.String;

    public ServerSelectorScene(EventHandler? createServerCallback) {
        this._servers = new ServerDictionary(this.ChangeStatusOfActions);
        TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Tcp, new RequestRoomsPacket());

        this._titleScene = new Text("Server Selector") {
            Position = new Vector2((TTGGame.Instance.GraphicManager.ScreenWidth / 2) - 50f, 25f)
        };

        this._createServerButton = new Button("Create server") {
            Position = new Vector2(TTGGame.Instance.GraphicManager.ScreenWidth - 200f, 5f),
        };

        this._createServerButton.Click += createServerCallback;

        this._nicknameText = new Text("Nickname") {
            Position = new Vector2(5f, TTGGame.Instance.GraphicManager.ScreenHeight - 45f)
        };

        this._nicknameTextField = new TextField(TTGGame.Instance.Nickname) {
            Position = new Vector2(this._nicknameText.Measures.X + 10f, TTGGame.Instance.GraphicManager.ScreenHeight - TTGGame.Instance.TextureManager.TextField.Height - 5f)
        };

        this._nicknameTextField.Change += Nickname_Change;
    }

    private void ChangeStatusOfActions(bool status) {
        this.BackButton.Disabled = status;
        this._createServerButton.Disabled = status;
        this._nicknameTextField.Disabled = status;

        foreach (var server in this._servers.Values)
            server.JoinButton.Disabled = status;
    }

    private static void ServerRow(GameTime gameTime, ServerData server) {
        var spriteBatch = TTGGame.Instance.SpriteBatch;

        spriteBatch.DrawCenter(
            TTGGame.Instance.TextureManager.TextField,
            new Vector2(0, server.Height),
            null,
            Color.White,
            0f,
            null,
            new Vector2(3.5f, 1f),
            SpriteEffects.None,
            0f
        );

        server.Title.Draw(gameTime);
        server.JoinButton.Draw(gameTime);
    }

    public void PacketReceivedCallback(Packet packet) {
        switch (packet) {
            case AddRoomPacket arp:
                this._servers.Add(new ServerRowData(arp));
                break;
            case RemoveRoomPacket rrp:
                this._servers.Remove(rrp.ID);
                break;
            case JoinRoomResultPacket jrrp:
                Console.WriteLine(jrrp.Message);
                this.ChangeStatusOfActions(false);
                break;
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this._nicknameTextField.Update(gameTime);
        this._createServerButton.Update(gameTime);

        foreach (var server in this._servers.Values.ToList())
            server.JoinButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
        this._titleScene.Draw(gameTime);
        this._createServerButton.Draw(gameTime);
        this._nicknameText.Draw(gameTime);
        this._nicknameTextField.Draw(gameTime);

        foreach (var server in this._servers.Values.ToList())
            ServerRow(gameTime, server);
    }

}