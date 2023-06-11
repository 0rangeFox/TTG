using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Effect = TTG_Game.Models.Graphics.Effect;
using Texture2D = TTG_Game.Models.Graphics.Texture2D;

namespace TTG_Game.Models; 

public class Sprite : DrawableComponent {

    private const float BorderThickness = 15.0f;
    private const float PixelWidth = 1.0f / BorderThickness;

    protected Texture2D Texture;
    protected bool IsFlipped = false;
    protected bool IsHighlighted = false;

    public Vector2 Position { get; set; } = Vector2.Zero;
    public Rectangle Rectangle => new((int) this.Position.X, (int) this.Position.Y, this.Texture.Width, this.Texture.Height);

    private readonly SpriteBatch _spriteBatch = new(TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice);

    public Sprite(Texture2D texture) {
        this.Texture = texture;
    }

    public override void Draw(GameTime gameTime) {
        this._spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: TTGGame.Instance.Scene.Camera?.Transform);

        if (IsHighlighted) {
            var effect = TTGGame.Instance.TextureManager.GetEffect(Effect.Highlight);
            effect.Parameters["texelSize"].SetValue(new Vector2(1f / (PixelWidth * this.Texture.Width), 1f / (PixelWidth * this.Texture.Height)));
            effect.Parameters["outlineColor"].SetValue(Color.Yellow.ToVector4());
            effect.CurrentTechnique.Passes[0].Apply();
        }

        this._spriteBatch.Draw(
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

        this._spriteBatch.End();
    }

}