using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Controls;
using Texture = TTG_Game.Models.Graphics.Texture;

namespace TTG_Game.Models; 

public delegate void BackCallBack(object? sender, EventArgs e);

public class SubScene : Scene {

    protected readonly Button BackButton;
    private event EventHandler? BackCallBack;

    public BackCallBack? BackCallback {
        set {
            if (value == null && this.BackCallBack != null) {
                this.BackButton.Click -= this.BackCallBack;
                this.BackCallBack = null;
            } else if (value != null) {
                if (this.BackCallBack != null)
                    this.BackButton.Click -= this.BackCallBack;

                this.BackCallBack = new EventHandler(value);
                this.BackButton.Click += this.BackCallBack;
            }
        }
    }

    protected SubScene() {
        this.BackButton = new Button(TTGGame.Instance.TextureManager.GetTexture(Texture.ArrowRight)) {
            Position = new Vector2(10f),
            Scale = new Vector2(.5f, 1f),
            Effects = SpriteEffects.FlipHorizontally
        };
    }

    public override void Update(GameTime gameTime) {
        this.BackButton.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        this.BackButton.Draw(gameTime);
    }

}