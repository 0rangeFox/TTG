using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Models; 

public class Sprite : DrawableComponent {

    protected Texture2D Texture;
    protected bool IsFlipped = false;

    public Vector2 Position { get; set; }
    public Rectangle Rectangle => new((int) this.Position.X, (int) this.Position.Y, this.Texture.Width, this.Texture.Height);

    public Sprite(Texture2D texture) {
        this.Texture = texture;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
        spriteBatch.Draw(
            this.Texture,
            this.Position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            0f
        );
    }

}