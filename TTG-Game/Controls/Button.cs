using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Utils.Extensions;

namespace TTG_Game.Controls; 

// Credits of original code: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial012/Controls/Button.cs
public class Button : Text {

    #region Fields

    private MouseState _currentMouse;
    private MouseState _previousMouse;
    private bool _isHovering;

    private readonly Texture2D _texture;
    private Vector2 _position = Vector2.Zero;

    #endregion

    #region Properties

    public Color OnHoverColor { get; set; }

    public bool Centered { get; set; }
    public new Vector2 Position {
        get => this._position;
        set {
            this._position = value;

            var rectangle = this.Rectangle;
            var x = (rectangle.X + (rectangle.Width / 2)) - (this.Font.MeasureString(this.String).X / 2);
            var y = (rectangle.Y + (rectangle.Height / 2)) - (this.Font.MeasureString(this.String).Y / 2);
            base.Position = new Vector2(x, y);
        }
    }
    public Rectangle Rectangle => this.Centered ?
        new Rectangle((int) ((TTGGame.Instance.GraphicManager.ScreenCenter.X - this._texture.Width / 2) + this._position.X), (int) ((TTGGame.Instance.GraphicManager.ScreenCenter.Y - this._texture.Height / 2) + this._position.Y), this._texture.Width, this._texture.Height) :
        new Rectangle((int) this._position.X, (int) this._position.Y, this._texture.Width, this._texture.Height);

    public event EventHandler Click;
    public bool Clicked { get; private set; }
    #endregion

    #region Methods

    public Button(string text) : this(TTGGame.Instance.TextureManager.Button, text) {}

    public Button(Texture2D texture, string text) : base(text) {
        this._texture = texture;

        this.Color = Color.White;
        this.OnHoverColor = this.Color.Darker(.75f);
    }

    public override void Update(GameTime gameTime) {
        this._previousMouse = this._currentMouse;
        this._currentMouse = Mouse.GetState();

        var mouseRectangle = new Rectangle(this._currentMouse.X, this._currentMouse.Y, 1, 1);

        this._isHovering = false;

        if (!mouseRectangle.Intersects(this.Rectangle)) return;

        this._isHovering = true;

        this.Clicked = this._currentMouse.LeftButton == ButtonState.Released && this._previousMouse.LeftButton == ButtonState.Pressed;
        if (this.Clicked) this.Click?.Invoke(this, EventArgs.Empty);
    }

    public override void Draw(GameTime gameTime) {
        this.SpriteBatch.Draw(
            this._texture,
            this.Rectangle,
            this._isHovering ? this.OnHoverColor : this.Color
        );

        base.Draw(gameTime);
    }

    #endregion
}