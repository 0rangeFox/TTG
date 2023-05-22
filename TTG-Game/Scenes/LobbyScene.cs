using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class LobbyScene : Scene {

    private readonly Character _character;
    private readonly SpriteBatch _spriteBatch = new(TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice);

    public LobbyScene() {
        this._character = new Character(Color.Orange);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.OrangeRed);

        _spriteBatch.Begin();
        _spriteBatch.Draw(this._character.Idle, Vector2.Zero, null, Color.White);
        _spriteBatch.End();
    }

}