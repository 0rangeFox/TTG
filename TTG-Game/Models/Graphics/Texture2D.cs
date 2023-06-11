using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTexture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace TTG_Game.Models.Graphics; 

public class Texture2D : XnaTexture2D {

    public Texture ID { get; init; }

    public Texture2D(Texture id, XnaTexture2D texture2D) : base(texture2D.GraphicsDevice, texture2D.Width, texture2D.Height) {
        this.ID = id;

        var data = new Color[this.Width * this.Height];
        texture2D.GetData(data);
        this.SetData(data);
    }

    public Texture2D(GraphicsDevice graphicsDevice, Texture id, int width, int height) : base(graphicsDevice, width, height) {
        this.ID = id;
    }

    public Texture2D(GraphicsDevice graphicsDevice, Texture id, int width, int height, bool mipmap, SurfaceFormat format) : base(graphicsDevice, width, height, mipmap, format) {
        this.ID = id;
    }

    public Texture2D(GraphicsDevice graphicsDevice, Texture id, int width, int height, bool mipmap, SurfaceFormat format, int arraySize) : base(graphicsDevice, width, height, mipmap, format, arraySize) {
        this.ID = id;
    }

    protected Texture2D(GraphicsDevice graphicsDevice, Texture id, int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type, bool shared, int arraySize) : base(graphicsDevice, width, height, mipmap, format, type, shared, arraySize) {
        this.ID = id;
    }

    public Texture2D Clone() {
        var clonedTexture = new Texture2D(this.GraphicsDevice, this.ID, this.Width, this.Height);

        var data = new Color[this.Width * this.Height];
        this.GetData(data);
        clonedTexture.SetData(data);

        return clonedTexture;
    }

}