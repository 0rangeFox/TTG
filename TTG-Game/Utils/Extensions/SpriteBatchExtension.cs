using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Utils.Extensions; 

public static class SpriteBatchExtension {

    public static void DrawCenter(
        this SpriteBatch spriteBatch,
        Texture2D texture,
        Rectangle destinationRectangle,
        Color color
    ) {
        spriteBatch.DrawCenter(
            texture,
            null, //new Vector2(destinationRectangle.X, destinationRectangle.Y),
            destinationRectangle,
            color,
            0f,
            null,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public static void DrawCenter(
        this SpriteBatch spriteBatch,
        Texture2D texture,
        Vector2? position,
        Color color
    ) {
        spriteBatch.DrawCenter(
            texture,
            position,
            null,
            color,
            0f,
            null,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public static void DrawCenter(
        this SpriteBatch spriteBatch,
        Texture2D texture,
        Vector2? position,
        Rectangle? sourceRectangle,
        Color color,
        float rotation,
        Vector2? origin,
        Vector2 scale,
        SpriteEffects effects,
        float layerDepth
    ) {
        spriteBatch.Draw(
            texture,
            TTGGame.Instance.GraphicManager.ScreenCenter + (position ?? Vector2.Zero),
            sourceRectangle,
            color,
            rotation,
            origin ?? new Vector2(texture.Width / 2f, texture.Height / 2f),
            scale,
            effects,
            layerDepth
        );
    }

}