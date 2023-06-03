using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;

namespace TTG_Game.Models; 

public delegate void BackCallBack(object? sender, EventArgs e);

public class SubScene : Scene {

    private event EventHandler? BackCallBack;

    public BackCallBack? BackCallback {
        set {
            if (value == null && this.BackCallBack != null) {
                this._backButton.Click -= this.BackCallBack;
                this.BackCallBack = null;
            } else if (value != null) {
                if (this.BackCallBack != null)
                    this._backButton.Click -= this.BackCallBack;

                this.BackCallBack = new EventHandler(value);
                this._backButton.Click += this.BackCallBack;
            }
        }
    }

    private readonly Button _backButton;

    protected SubScene() {
        this._backButton = new Button(TTGGame.Instance.TextureManager.ArrowRight) {
            Position = new Vector2(10f),
            Scale = new Vector2(.5f, 1f),
            Effects = SpriteEffects.FlipHorizontally
        };
    }

    public override void Update(GameTime gameTime) {
        this._backButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        this._backButton.Draw(gameTime);
    }

}