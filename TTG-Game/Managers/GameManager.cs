using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;
using TTG_Game.Networking;
using TTG_Game.Scenes;
using TTG_Shared;

namespace TTG_Game.Managers; 

public class GameManager {

    public static int ScreenWidth;
    public static int ScreenHeight;

    public readonly Game Game;
    public readonly GraphicsDeviceManager GraphicsDeviceManager;
    public SpriteBatch SpriteBatch;

    public readonly FontManager FontManager = new();
    public readonly TextureManager TextureManager = new();
    public readonly NetworkManager NetworkManager = new();

    public Scene Scene;

    public GameManager(Game game) {
        this.Game = game;
        this.GraphicsDeviceManager = new GraphicsDeviceManager(this.Game);

        ScreenWidth = this.GraphicsDeviceManager.PreferredBackBufferWidth;
        ScreenHeight = this.GraphicsDeviceManager.PreferredBackBufferHeight;
    }

    public void Initialize() {
        this.Game.Window.Title = $"The Traitor's Gambit v{GitInformation.Version} ({GitInformation.ShortSha})";
        this.Game.Content.RootDirectory = "Content";
        this.Game.IsMouseVisible = true;

        this.SpriteBatch = new SpriteBatch(this.GraphicsDeviceManager.GraphicsDevice);

        this.FontManager.Load();
        this.TextureManager.Load();

        this.Scene = new MainScene();
    }

    public T Load<T>(string assetName) => this.Game.Content.Load<T>(assetName);

}