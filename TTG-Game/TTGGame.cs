using Microsoft.Xna.Framework;
using TTG_Game.Managers;
using TTG_Game.Utils;

namespace TTG_Game;

public class TTGGame : Game {

    public static GameManager Instance;

    public TTGGame() {
        Instance = new GameManager(this);
    }

    protected override void Initialize() {
        base.Initialize();
        Instance.Initialize();
    }

    protected override void Update(GameTime gameTime) {
        KeyboardUtil.Update(gameTime);
        Instance.Scene.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        Instance.SpriteBatch.Begin(transformMatrix: Instance.Scene.Camera?.Transform);
        Instance.Scene.Draw(gameTime);
        Instance.SpriteBatch.End();
    }

}