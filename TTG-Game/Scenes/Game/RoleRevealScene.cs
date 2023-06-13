using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Models;
using TTG_Game.Utils.Extensions;
using TTG_Shared.Models;
using TTG_Shared.Packets;
using TTG_Shared.Utils.Extensions;
using Texture = TTG_Game.Models.Graphics.Texture;
using Texture2D = TTG_Game.Models.Graphics.Texture2D;

namespace TTG_Game.Scenes; 

public class RoleRevealScene : SubScene {

    private readonly Roles _role;
    private double _totalSeconds = 0;
    private bool _isReady = false;

    private readonly Texture2D _revealRoleImage = TTGGame.Instance.TextureManager.GetTexture(Texture.RevealRole);

    public RoleRevealScene(Roles role) {
        this._role = role;
    }

    private void SetReadyToPlay() => TTGGame.Instance.NetworkManager.SendPacket(new ReadyRoomPacket());

    public override void Update(GameTime gameTime) {
        if (this._isReady) return;

        this._totalSeconds += gameTime.ElapsedGameTime.TotalSeconds;
        if (this._isReady = this._totalSeconds >= 3d)
            this.SetReadyToPlay();
    }

    public override void Draw(GameTime gameTime) {
        TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

        if (this._totalSeconds <= 1.5d)
            TTGGame.Instance.SpriteBatch.DrawCenter(this._revealRoleImage, null, null, Color.White, 0f, null, new Vector2(.1f), SpriteEffects.None, 0);
        else
            TTGGame.Instance.SpriteBatch.DrawStringCenter(TTGGame.Instance.FontManager.AmongUs128px, this._role.GetString(), null, this._role.GetColor().ToXna());
    }

}