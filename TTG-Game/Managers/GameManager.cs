using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;
using TTG_Game.Networking;
using TTG_Game.Scenes;
using TTG_Shared;

namespace TTG_Game.Managers; 

public class GameManager {

    public readonly Game Game;
    public readonly GraphicsDeviceManager GraphicsDeviceManager;
    public readonly GraphicManager GraphicManager;
    public SpriteBatch SpriteBatch;

    public readonly FontManager FontManager = new();
    public readonly TextureManager TextureManager = new();
    public readonly NetworkManager NetworkManager = new("127.0.0.1", 7325);

    public string Nickname;
    public Scene Scene;

    public readonly List<IEntity> Entities = new();
    public readonly List<IEntity> NearbyEntities = new();

    public GameManager(Game game) {
        this.Game = game;
        this.GraphicsDeviceManager = new GraphicsDeviceManager(this.Game);
        this.GraphicManager = new GraphicManager(this.GraphicsDeviceManager, new Vector2(3840, 2160));
    }

    public void Initialize() {
        this.Game.Window.Title = $"The Traitor's Gambit v{GitInformation.Version} ({GitInformation.ShortSha})";
        this.Game.Content.RootDirectory = "Content";
        this.Game.IsMouseVisible = true;

        this.SpriteBatch = new SpriteBatch(this.GraphicsDeviceManager.GraphicsDevice);
        this.GraphicManager.Update();

        this.FontManager.Load();
        this.TextureManager.Load();

        this.Nickname = "0rangeFox";
        this.Scene = new MainScene();
    }

    public T Load<T>(string assetName) => this.Game.Content.Load<T>(assetName);

}