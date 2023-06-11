using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Managers;
using TTG_Game.Utils;
using TTG_Shared.Packets;

namespace TTG_Game.Models; 

public class Player : AnimatedEntity {

    private const float DetectionRadius = 500.0f;
    private const float Speed = 10f;

    public string Nickname;

    public new Vector2 Position {
        get => base.Position;
        set {
            if (base.Position.Equals(value)) return;
            base.Position = value;

            if (!this._isNetwork)
                TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Udp, new PlayerMovementPacket(base.Position.ToNumerics()));
        }
    }

    private Character _character;
    private bool _isNetwork;

    private Vector2 _velocity = Vector2.Zero;
    private IEntity? _selectingNearbyEntity;

    public Player(string nickname, Color color, Vector2 position, bool isNetwork = false) : base(new List<Texture2D>() { TextureManager.Empty }) {
        this.Nickname = nickname;
        this.Position = position;
        this._character = new Character(color);
        this._isNetwork = isNetwork;

        this.Texture = this._character.Idle;
        this.Textures = this._character.Walk;
    }

    private Vector2 GenerateTextCenterCoords(SpriteFont font, string text) {
        var fontMeasures = font.MeasureString(text);
        var centerX = this.Position.X + (this.Texture.Width - fontMeasures.X) / 2;
        var centerY = this.Position.Y + (this.Texture.Height - fontMeasures.Y) / 2;
        return new Vector2(centerX, centerY);
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

                this._selectingNearbyEntity ??= entity;
                entity.Highlight = entity == this._selectingNearbyEntity;
            } else if (entity == this._selectingNearbyEntity) {
                this._selectingNearbyEntity = null;
                entity.Highlight = false;
            }
        }
    }

    private void CheckKeyboard() {
        if (KeyboardUtil.IsGoingDown(Keys.Tab)) {
            var nearbyEntities = TTGGame.Instance.NearbyEntities;
            if (nearbyEntities.Count > 0) {
                if (this._selectingNearbyEntity != null && nearbyEntities.Contains(this._selectingNearbyEntity)) {
                    var nextIndex = (nearbyEntities.IndexOf(this._selectingNearbyEntity) + 1) % nearbyEntities.Count;
                    this._selectingNearbyEntity = nearbyEntities[nextIndex];
                } else this._selectingNearbyEntity = nearbyEntities[0];
            } else this._selectingNearbyEntity = null;
        }

        if (this._selectingNearbyEntity != null && KeyboardUtil.IsGoingDown(Keys.E))
            this._selectingNearbyEntity.ExecuteAction();

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

    private void DrawSelectedNearbyEntity() {
        if (this._selectingNearbyEntity == null) return;

        var entity = (Sprite)TTGGame.Instance.NearbyEntities[TTGGame.Instance.NearbyEntities.IndexOf(this._selectingNearbyEntity)];
        TTGGame.Instance.SpriteBatch.Draw(
            TTGGame.Instance.TextureManager.Report,
            entity.Position + new Vector2(entity.Rectangle.Width - 100, -100),
            null,
            Color.White,
            0f,
            Vector2.Zero,
            new Vector2(2.5f),
            SpriteEffects.None,
            0
        );
    }

    public override void Update(GameTime gameTime) {
        if (_isNetwork) return;

        this.CheckKeyboard();

        this.Position += _velocity;
        this._velocity = Vector2.Zero;

        this.CheckNearbyEntities();

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.SpriteBatch.DrawString(
            TTGGame.Instance.FontManager.AmongUs128px,
            this.Nickname,
            this.GenerateTextCenterCoords(TTGGame.Instance.FontManager.AmongUs128px, this.Nickname) - new Vector2(0f, 325f),
            Color.Blue
        );

        this.DrawSelectedNearbyEntity();

        base.Draw(gameTime);
    }

}