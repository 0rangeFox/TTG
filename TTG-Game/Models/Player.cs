using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Managers;
using TTG_Game.Scenes;
using TTG_Game.Utils;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;
using TTG_Shared.Utils.Extensions;
using Texture = TTG_Game.Models.Graphics.Texture;
using Texture2D = TTG_Game.Models.Graphics.Texture2D;

namespace TTG_Game.Models; 

public class Player : AnimatedEntity {

    private const float MaxVolumeDistance = 1000.0f;
    private const float DetectionRadius = 500.0f;
    private const float Speed = 10.0f;
    private const float MaxFootstepsVolume = .45f;

    private Roles _role = Roles.Citizen;
    private Color _roleColor = Color.White;

    public Guid ID;
    public string Nickname;
    public bool IsDead;

    public Roles Role {
        get => this._role;
        set {
            this._role = value;
            this._roleColor = this._role.GetColor().ToXna();
        }
    }

    public new Vector2 Position {
        get => base.Position;
        set {
            if (base.Position.Equals(value)) return;
            base.Position = value;

            if (!this._isNetwork)
                this.SendUpdatedPositionPacket();
        }
    }

    private Character _character;
    private bool _isNetwork;

    private Vector2 _velocity = Vector2.Zero;
    private IEntity? _selectingNearbyEntity;

    private readonly SoundEffectInstance _footstepsSound = TTGGame.Instance.AudioManager.GetSound(Sound.Footsteps);

    public Player(Guid id, string nickname, Color color, Vector2 position, bool isNetwork = false) : base(new List<Texture2D>() { TextureManager.Empty }) {
        this.ID = id;
        this.Nickname = nickname;
        this.Position = position;
        this._character = new Character(color);
        this._isNetwork = isNetwork;

        this.Texture = this._character.Idle;
        this.Textures = this._character.Walk;

        this._footstepsSound.IsLooped = true;
        this._footstepsSound.Volume = MaxFootstepsVolume;
    }

    private void SendUpdatedPositionPacket() => TTGGame.Instance.NetworkManager.SendPacket(new PlayerMovementPacket(base.Position.ToNumerics(), (ushort) this.Texture.ID, this.IsFlipped));

    private Texture2D GetNetworkedTexture(Texture id) {
        if (this._character.Idle.ID.Equals(id)) return this._character.Idle;
        if (this._character.Dead.ID.Equals(id)) return this._character.Dead;
        return this._character.Walk.FirstOrDefault(texture => texture.ID.Equals(id)) ?? this._character.Idle;
    }

    public void UpdatePosition(PlayerMovementPacket packet) {
        if (!this._isNetwork) return;
        this.Position = packet.Position;
        this.Texture = this.GetNetworkedTexture((Texture) packet.Texture);
        this.IsFlipped = packet.Direction;

        if (this.Texture.ID.Equals(Graphics.Texture.CharacterIdle))
            this._footstepsSound.Stop();
        else this._footstepsSound.Play();
    }

    public void Kill() {
        if (this.IsDead) return;
        this.IsDead = true;
        this.Texture = this._character.Dead;
        this._footstepsSound.Stop();
        TTGGame.Instance.AudioManager.Play(Sound.Kill);
    }

    private Vector2 GenerateTextCenterCoords(SpriteFont font, string text) {
        var fontMeasures = font.MeasureString(text);
        var centerX = this.Position.X + (this.Texture.Width - fontMeasures.X) / 2;
        var centerY = this.Position.Y + (this.Texture.Height - fontMeasures.Y) / 2;
        return new Vector2(centerX, centerY);
    }

    private void CheckRelationshipByPlayer(IEntity entity) {
        if (entity is not Player p) return;

        if (this.Role == Roles.Traitor && !p.IsDead && !entity.ActionTexture.ID.Equals(Graphics.Texture.Kill))
            entity.ActionTexture = TTGGame.Instance.TextureManager.GetTexture(Graphics.Texture.Kill);
        else if (p.IsDead && !entity.ActionTexture.ID.Equals(Graphics.Texture.Report))
            entity.ActionTexture = TTGGame.Instance.TextureManager.GetTexture(Graphics.Texture.Report);
    }

    private void CheckNearbySounds() {
        if (TTGGame.Instance.Scene is not GameScene scene) return;
        var viewerPlayer = scene.Players.First();
        foreach (var player in scene.Players) {
            if (player.IsDead || viewerPlayer.Equals(player)) continue;
            var distance = Vector2.Distance(player.Position, viewerPlayer.Position);
            player._footstepsSound.Volume = MathHelper.Clamp(1f - (distance / MaxVolumeDistance), 0f, MaxFootstepsVolume);
        }
    }

    private void CheckNearbyEntities() {
        var nearbyEntities = TTGGame.Instance.NearbyEntities;
        nearbyEntities.Clear();

        foreach (var entity in TTGGame.Instance.Entities) {
            if (entity == this) continue;

            var distance = Vector2.Distance(this.Position, ((Sprite) entity).Position);

            if (distance <= DetectionRadius) {
                this.CheckRelationshipByPlayer(entity);

                nearbyEntities.Add(entity);

                this._selectingNearbyEntity ??= entity;
                entity.Highlight = entity.ActionTexture.ID != Graphics.Texture.Empty && entity == this._selectingNearbyEntity;
            } else if (entity == this._selectingNearbyEntity) {
                this._selectingNearbyEntity = null;
                entity.Highlight = false;
                entity.ActionTexture = TextureManager.Empty;
            }
        }
    }

    private void StartWalking() {
        this._footstepsSound.Play();
    }

    private void StopWalking() {
        this.SendUpdatedPositionPacket();
        this._footstepsSound.Stop();
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

        if (Keyboard.GetState().IsKeyDown(Keys.W)) this._velocity.Y += -Speed;
        if (Keyboard.GetState().IsKeyDown(Keys.S)) this._velocity.Y += Speed;
        if (Keyboard.GetState().IsKeyDown(Keys.A)) this.IsFlipped = (this._velocity.X += -Speed) < 0;
        if (Keyboard.GetState().IsKeyDown(Keys.D)) this.IsFlipped = (this._velocity.X += Speed) < 0;

        if (this._velocity.Equals(Vector2.Zero))
            this.StopAnimation(this._character.Idle, this.StopWalking);
        else this.PlayAnimation(this.StartWalking);
    }

    private void DrawSelectedNearbyEntity() {
        if (this._selectingNearbyEntity == null) return;

        var entity = TTGGame.Instance.NearbyEntities[TTGGame.Instance.NearbyEntities.IndexOf(this._selectingNearbyEntity)];
        var entitySprite = (Sprite) entity;

        TTGGame.Instance.SpriteBatch.Draw(
            entity.ActionTexture,
            entitySprite.Position + new Vector2(entitySprite.Rectangle.Width - 100, -100),
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
        if (this.IsDead || this._isNetwork) return;

        this.CheckKeyboard();

        this.Position += _velocity;
        this._velocity = Vector2.Zero;

        this.CheckNearbySounds();
        this.CheckNearbyEntities();

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        if (!this.IsDead) {
            TTGGame.Instance.SpriteBatch.DrawString(
                TTGGame.Instance.FontManager.AmongUs128px,
                this.Nickname,
                this.GenerateTextCenterCoords(TTGGame.Instance.FontManager.AmongUs128px, this.Nickname) - new Vector2(0f, 325f),
                this._roleColor
            );

            this.DrawSelectedNearbyEntity();
        }

        base.Draw(gameTime);
    }

}