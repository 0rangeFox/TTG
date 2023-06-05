using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;

namespace TTG_Game.Scenes;

public class ServerSelectorScene : SubScene {

    private class ServerData : ServerRowData {

        public float Height;

        public ServerData(ServerRowData server, float height) : base(server) {
            this.Height = height;
        }

    }

    private class ServerDictionary {

        private readonly Dictionary<Guid, ServerData> _dictionary = new();

        public Dictionary<Guid, ServerData>.ValueCollection Values => this._dictionary.Values;

        public void Add(ServerRowData server) {
            this._dictionary.Add(server.ID, new ServerData(server, this._dictionary.Values.Count > 0 ? this._dictionary.Values.Last().Height + 55 : -125));
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

    private readonly ServerDictionary _servers = new();

    private void Nickname_Change(object? sender, EventArgs e) => TTGGame.Instance.Nickname = this._nicknameTextField.String;

    public ServerSelectorScene(EventHandler? createServerCallback) {
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

        this._servers.Add(new ServerRowData(Guid.NewGuid(), "0rangeFoxys best server omegalul", 1, 6, 1));
        this._servers.Add(new ServerRowData(Guid.NewGuid(), "IPCA Test", 4, 12, 2));
        this._servers.Add(new ServerRowData(Guid.NewGuid(), "Lets go, join here!", 3, 16, 5));
    }

    private static void ServerRow(ServerData server) {
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
        
        spriteBatch.DrawCenter(
            TTGGame.Instance.TextureManager.ArrowRight,
            new Vector2(275f, server.Height),
            null,
            Color.White,
            0f,
            null,
            new Vector2(.45f, .55f),
            SpriteEffects.None,
            0f
        );

        var serverText = server.Text.Length > 25 ? server.Text[..25] + "..." : server.Text;
        var text = $"{serverText} | Max Traitors: {server.MaxImpostors} | Players: {server.Players} / {server.MaxPlayers}".ToLower();
        var textMeasures = TTGGame.Instance.FontManager.AmongUs24px.MeasureString(text);
        spriteBatch.DrawStringCenter(
            TTGGame.Instance.FontManager.AmongUs24px,
            text,
            new Vector2(-300f + textMeasures.X / 2, server.Height),
            Color.White
        );
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this._nicknameTextField.Update(gameTime);
        this._createServerButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
        this._titleScene.Draw(gameTime);
        this._createServerButton.Draw(gameTime);
        this._nicknameText.Draw(gameTime);
        this._nicknameTextField.Draw(gameTime);

        foreach (var server in this._servers.Values)
            ServerRow(server);
    }

}