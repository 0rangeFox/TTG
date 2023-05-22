using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG_Game.Utils.Extensions; 

public static class Texture2DExtension {

    public static Texture2D Clone(this Texture2D texture) {
        var clonedTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);

        var data = new Color[texture.Width * texture.Height];
        texture.GetData(data);
        clonedTexture.SetData(data);

        return clonedTexture;
    }

}