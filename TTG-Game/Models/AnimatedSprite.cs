using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TTG_Game.Models.Graphics;

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

    public void PlayAnimation(Action? onStartCB = null) {
        if (!this._isPaused) return;
        this._isPaused = false;
        this.Texture = this.Textures[0];
        onStartCB?.Invoke();
    }

    public void StopAnimation(Texture2D texture, Action? onStopCB = null) {
        if (this._isPaused) return;
        this._isPaused = true;
        this._timer = 0f;
        this.Texture = texture;
        onStopCB?.Invoke();
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