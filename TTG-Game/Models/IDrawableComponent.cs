using Microsoft.Xna.Framework;

namespace TTG_Game.Models; 

public interface IDrawableComponent {

    public void Update(GameTime gameTime);
    public void Draw(GameTime gameTime);

}