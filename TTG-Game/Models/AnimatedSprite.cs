using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Models; 

public class AnimatedSprite : Sprite {

    private const double Delay = .15f;

    protected List<Texture2D> Textures;
    private int _currentTexture;

    private bool _isPaused = true;
    private double _timer;

    public AnimatedSprite(List<Texture2D> textures) : base(textures[0]) {
        this.Textures = textures;
    }

    public void PlayAnimation() {
        if (!this._isPaused) return;
        this._isPaused = false;
        this.Texture = this.Textures[0];
    }

    public void StopAnimation(Texture2D texture) {
        if (this._isPaused) return;
        this._isPaused = true;
        this._timer = 0f;
        this.Texture = texture;
    }

    public override void Update(GameTime gameTime) {
        if (!this._isPaused) {
            this._timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (this._timer > Delay) {
                this._currentTexture = (this._currentTexture + 1) % this.Textures.Count;
                this._timer = 0f;
                this.Texture = this.Textures[this._currentTexture];
            }
        }

        base.Update(gameTime);
    }

}