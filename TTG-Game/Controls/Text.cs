using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;

namespace TTG_Game.Controls; 

public class Text : DrawableComponent {

    #region Fields

    private bool _isStatic = false;

    #endregion

    #region Properties

    protected SpriteBatcheable SpriteBatch { get; } = new();
    public SpriteFont Font { get; set; } = TTGGame.Instance.FontManager.AmongUs24px;
    public string String { get; set; }
    public Vector2 Position { get; set; } = Vector2.Zero;
    /**
     * The angle is in Radians.
     * https://images.squarespace-cdn.com/content/v1/57c0d8d1e58c622e8b6d5328/84093a5e-6c4d-4cf4-b6d7-5910e7faac9d/Unit%2Bcircle%2Bwith%2Bdegrees%2Band%2Bradians.png
     */
    public float Rotation { get; set; } = 0f;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Color Color { get; set; } = Color.White;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; } = 0f;
    public Vector2 Measures => this.Font.MeasureString(this.String);

    public bool Static {
        get => this._isStatic;
        set {
            this._isStatic = value;
            this.SpriteBatch.Static = this._isStatic;
        }
    }

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
            this.Origin,
            this.Scale,
            this.Effects,
            this.LayerDepth
        );
    }

    #endregion

}