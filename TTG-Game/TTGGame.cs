using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Managers;

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
        Instance.Scene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        Instance.SpriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: Instance.Scene.Camera?.Transform);
        Instance.Scene.Draw(gameTime);
        Instance.SpriteBatch.End();
    }

}