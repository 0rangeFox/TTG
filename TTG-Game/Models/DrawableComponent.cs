using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Models;

public abstract class DrawableComponent {

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {}

}