using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Texture2D = TTG_Game.Models.Graphics.Texture2D;

namespace TTG_Game.Models;

public class SpriteBatcheable : SpriteBatch {

    #region Fields

    private readonly SpriteBatch _spriteBatch = TTGGame.Instance.SpriteBatch;

    #endregion
    
    #region Properties

    public bool Static { get; set; } = false;

    #endregion
    
    #region Methods

    public SpriteBatcheable() : base(TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice) {}

    public new void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color color,
        float rotation,
        Vector2 origin,
        float scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color color,
        float rotation,
        Vector2 origin,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
            return;
        }

        this.Begin();
        base.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Vector2 position,
        Rectangle? sourceRectangle,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, position, sourceRectangle, color);
            return;
        }

        this.Begin();
        base.Draw(texture, position, sourceRectangle, color);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle? sourceRectangle,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
            return;
        }

        this.Begin();
        base.Draw(texture, destinationRectangle, sourceRectangle, color);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Vector2 position,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, position, color);
            return;
        }

        this.Begin();
        base.Draw(texture, position, color);
        this.End();
    }

    public new void Draw(
        Texture2D texture,
        Rectangle destinationRectangle,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.Draw(texture, destinationRectangle, color);
            return;
        }

        this.Begin();
        base.Draw(texture, destinationRectangle, color);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color);
        this.End();
    }

    public new void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        float scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth,
        bool rtl
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, rtl);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, rtl);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        StringBuilder text,
        Vector2 position,
        Color color
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color);
        this.End();
    }

    public new void DrawString(
        SpriteFont spriteFont,
        StringBuilder text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        float scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        StringBuilder text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        this.End();
    }

    public new unsafe void DrawString(
        SpriteFont spriteFont,
        StringBuilder text,
        Vector2 position,
        Color color,
        float rotation,
        Vector2 origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth,
        bool rtl
    ) {
        if (!this.Static) {
            this._spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, rtl);
            return;
        }

        this.Begin();
        base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, rtl);
        this.End();
    }

    #endregion

}