using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Managers; 

public class FontManager {

    public SpriteFont AmongUs { get; private set; }

    public void Load() {
        this.AmongUs = TTGGame.Instance.Load<SpriteFont>("Fonts/AmongUs");
    }

}