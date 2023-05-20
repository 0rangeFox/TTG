using Microsoft.Xna.Framework;
using TTG_Game.Models;

namespace TTG_Game.Scenes; 

public class LobbyScene : Scene {

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.OrangeRed);
    }

}