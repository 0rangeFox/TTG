using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Utils.Extensions;
using Texture = TTG_Game.Models.Graphics.Texture;
using Texture2D = TTG_Game.Models.Graphics.Texture2D;

namespace TTG_Game.Controls; 

// The idea of the logic of this code was obtained through: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial012/Controls/Button.cs
public class Button : Text {

    #region Fields

    private MouseState _currentMouse;
    private MouseState _previousMouse;
    private bool _isHovering;

    private readonly Texture2D _texture;
    private bool _isCentered = false;
    private Vector2 _position = Vector2.Zero;
    private Color _disabledColor;

    #endregion

    #region Properties

    public new Vector2 Origin { get; set; } = Vector2.Zero;

    public new Vector2 Scale { get; set; } = Vector2.One;

    public new SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public Color OnHoverColor { get; set; }

    public bool Centered {
        get => this._isCentered;
        set {
            this._isCentered = value;
            this._position = this._isCentered ? new Vector2(TTGGame.Instance.GraphicManager.ScreenCenter.X - this._texture.Width * this.Scale.X / 2, TTGGame.Instance.GraphicManager.ScreenCenter.Y - this._texture.Height * this.Scale.Y / 2) : Vector2.Zero;
        }
    }

    public new Vector2 Position {
        get => this._position;
        set {
            if (!this._position.Equals(value))
                this._position = this.Centered ? this._position + value : value;

            var fontMeasures = this.Font.MeasureString(this.String);
            base.Position = new Vector2(
                this._position.X + this._texture.Width * this.Scale.X / 2 - fontMeasures.X / 2,
                this._position.Y + this._texture.Height * this.Scale.Y / 2 - fontMeasures.Y / 2
            );
        }
    }

    public Rectangle Rectangle {
        get {
            // Calculate the rotation matrix
            var transform = Matrix.CreateScale(Scale.X, Scale.Y, 1) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(_position.X, _position.Y, 0);

            // Transform the four corners of the texture
            var topLeft = Vector2.Transform(Vector2.Zero, transform);
            var topRight = Vector2.Transform(new Vector2(_texture.Width, 0), transform);
            var bottomLeft = Vector2.Transform(new Vector2(0, _texture.Height), transform);
            var bottomRight = Vector2.Transform(new Vector2(_texture.Width, _texture.Height), transform);

            // Find the minimum and maximum coordinates
            var minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            var minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            var maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            var maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new Rectangle(
                (int) minX,
                (int) minY,
                (int) (maxX - minX),
                (int) (maxY - minY)
            );
        }
    }

    public event EventHandler Click;

    public bool Clicked { get; private set; }

    public bool Disabled { get; set; }

    #endregion

    #region Methods

    public Button(string text = "") : this(TTGGame.Instance.TextureManager.GetTexture(Texture.Button), text) {}

    public Button(Texture2D texture, string text = "") : base(text) {
        this._texture = texture;

        this.Color = Color.White;
        this.OnHoverColor = this.Color.Darker(.75f);
        this._disabledColor = this.Color.Darker(.5f);
    }

    public override void Update(GameTime gameTime) {
        if (this.Disabled) return;

        this._previousMouse = this._currentMouse;
        this._currentMouse = Mouse.GetState();

        this._isHovering = false;
        if (!new Rectangle(this._currentMouse.X, this._currentMouse.Y, 1, 1).Intersects(this.Rectangle)) return;
        this._isHovering = true;

        this.Clicked = this._currentMouse.LeftButton == ButtonState.Released && this._previousMouse.LeftButton == ButtonState.Pressed;
        if (this.Clicked) this.Click?.Invoke(this, EventArgs.Empty);
    }

    public override void Draw(GameTime gameTime) {
        this.SpriteBatch.Draw(
            this._texture,
            this._position,
            null,
            this.Disabled ? this._disabledColor : this._isHovering ? this.OnHoverColor : this.Color,
            this.Rotation,
            this.Origin,
            this.Scale,
            this.Effects,
            this.LayerDepth
        );

        base.Draw(gameTime);
    }

    #endregion
}