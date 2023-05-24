using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Managers;

namespace TTG_Game.Models; 

public class Player : AnimatedEntity {

    private const float DetectionRadius = 500.0f;
    private const float Speed = 10f;

    public string Nickname;
    private Character _character;
    private bool _isNetwork;

    private Vector2 _velocity = Vector2.Zero;

    public Player(string nickname, Color color, bool isNetwork = false) : base(new List<Texture2D>() { TextureManager.Empty }) {
        this.Nickname = nickname;
        this._character = new Character(color);
        this._isNetwork = isNetwork;

        this.Texture = this._character.Idle;
        this.Textures = this._character.Walk;
    }

    private void CheckNearbyEntities() {
        var nearbyEntities = TTGGame.Instance.NearbyEntities;
        nearbyEntities.Clear();

        foreach (var entity in TTGGame.Instance.Entities) {
            var entitySprite = (Sprite) entity;
            if (entity == this) continue;

            var distance = Vector2.Distance(this.Position, entitySprite.Position);

            if (distance <= DetectionRadius) {
                nearbyEntities.Add(entity);
                entity.Highlight = true;
            } else entity.Highlight = false;
        }
    }

    private void CheckKeyboard() {
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
    }

    public override void Update(GameTime gameTime) {
        if (_isNetwork) return;

        this.CheckNearbyEntities();
        this.CheckKeyboard();

        this.Position += _velocity;
        this._velocity = Vector2.Zero;

        base.Update(gameTime);
    }

}