using Microsoft.Xna.Framework;

namespace TTG_Game.Models;

public abstract class DrawableComponent {

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(GameTime gameTime) {}

}