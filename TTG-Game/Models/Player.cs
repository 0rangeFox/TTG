using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Managers;

namespace TTG_Game.Models; 

public class Player : AnimatedSprite {

    private const float Speed = 10f;

    private string _nickname;
    private Character _character;
    private bool _isNetwork;

    private Vector2 _velocity = Vector2.Zero;

    public Player(string nickname, Color color, bool isNetwork = false) : base(new List<Texture2D>() { TextureManager.Empty }) {
        this._nickname = nickname;
        this._character = new Character(color);
        this._isNetwork = isNetwork;

        this.Texture = this._character.Idle;
        this.Textures = this._character.Walk;
    }

    public override void Update(GameTime gameTime) {
        if (_isNetwork) return;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            this._velocity.Y = -Speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
            this._velocity.Y = Speed;

        if (Keyboard.GetState().IsKeyDown(Keys.A))
            this._velocity.X = -Speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            this._velocity.X = Speed;

        if (this._velocity == Vector2.Zero)
            this.StopAnimation(this._character.Idle);
        else {
            this.IsFlipped = this._velocity.X < 0;
            this.PlayAnimation();
        }

        this.Position += _velocity;
        this._velocity = Vector2.Zero;

        base.Update(gameTime);
    }

}