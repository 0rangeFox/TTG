using Microsoft.Xna.Framework;
using TTG_Game.Networking;
using TTG_Shared;

namespace TTG_Game.Managers; 

public class GameManager {

    private readonly Game _game;
    public readonly GraphicsDeviceManager GraphicsDeviceManager;
    public readonly FontManager FontManager = new();
    public readonly NetworkManager NetworkManager = new();

    public GameManager(Game game) {
        this._game = game;
        this.GraphicsDeviceManager = new GraphicsDeviceManager(this._game);
    }

    public void Initialize() {
        this._game.Window.Title = $"The Traitor's Gambit v{GitInformation.Version} ({GitInformation.ShortSha})";
        this._game.Content.RootDirectory = "Content";
        this._game.IsMouseVisible = true;

        this.FontManager.Load();
    }

    public T Load<T>(string assetName) => this._game.Content.Load<T>(assetName);

}