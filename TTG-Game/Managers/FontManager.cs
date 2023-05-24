using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Managers; 

public class FontManager {

    public SpriteFont AmongUs24px { get; private set; }
    public SpriteFont AmongUs128px { get; private set; }

    public void Load() {
        this.AmongUs24px = TTGGame.Instance.Load<SpriteFont>("Fonts/AmongUs-24px");
        this.AmongUs128px = TTGGame.Instance.Load<SpriteFont>("Fonts/AmongUs-128px");
    }

}