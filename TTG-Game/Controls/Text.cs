using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;

namespace TTG_Game.Controls; 

public class Text : DrawableComponent {

    #region Fields

    protected readonly SpriteBatch SpriteBatch = TTGGame.Instance.SpriteBatch;

    #endregion

    #region Properties

    public SpriteFont Font { get; set; } = TTGGame.Instance.FontManager.AmongUs24px;
    public string String { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    /**
     * The angle is in Radians.
     * https://images.squarespace-cdn.com/content/v1/57c0d8d1e58c622e8b6d5328/84093a5e-6c4d-4cf4-b6d7-5910e7faac9d/Unit%2Bcircle%2Bwith%2Bdegrees%2Band%2Bradians.png
     */
    public float Rotation { get; set; } = 0f;
    public Color Color { get; set; } = Color.White;
    public Vector2 Measures => this.Font.MeasureString(this.String);

    #endregion

    #region Methods

    public Text(string text = "") {
        this.String = text;
    }

    public override void Draw(GameTime gameTime) {
        if (string.IsNullOrEmpty(this.String)) return;
        this.SpriteBatch.DrawString(
            this.Font,
            this.String,
            this.Position,
            this.Color,
            this.Rotation,
            Vector2.Zero,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    #endregion

}