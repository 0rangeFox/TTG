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
    public Color Color { get; set; } = Color.White;
    public Vector2 Measures => this.Font.MeasureString(this.String);
    #endregion

    public Text(string text) {
        this.String = text;
    }

    public override void Draw(GameTime gameTime) {
        if (string.IsNullOrEmpty(this.String)) return;
        this.SpriteBatch.DrawString(this.Font, this.String, this.Position, this.Color);
    }

}