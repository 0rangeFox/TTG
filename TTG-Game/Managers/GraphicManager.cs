using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TTG_Game.Managers; 

// Credits of original code: https://stackoverflow.com/a/20813250/9379900
public class GraphicManager {

    private Vector3 _scalingFactor;

    public int ScreenWidth;
    public int ScreenHeight;
    public Vector2 ScreenCenter;

    /// <summary>
    /// The virtual screen size. Default is 1280x800. See the non-existent documentation on how this works.
    /// </summary>
    public Vector2 VirtualScreen;

    /// <summary>
    /// The screen scale
    /// </summary>
    public Vector2 ScreenAspectRatio = new(1, 1);

    /// <summary>
    /// The scale used for beginning the SpriteBatch.
    /// </summary>
    public Matrix Scale;

    /// <summary>
    /// The scale result of merging VirtualScreen with WindowScreen.
    /// </summary>
    public Vector2 ScreenScale;

    public GraphicManager(GraphicsDeviceManager graphicsDeviceManager, Vector2? virtualScreen = null) {
        this.ScreenWidth = graphicsDeviceManager.PreferredBackBufferWidth;
        this.ScreenHeight = graphicsDeviceManager.PreferredBackBufferHeight;
        this.VirtualScreen = virtualScreen ?? new Vector2(1280, 800);
    }

    /// <summary>
    /// Updates the specified graphics device to use the configured resolution.
    /// </summary>
    public void Update() {
        var device = TTGGame.Instance.GraphicsDeviceManager;
        this.ScreenWidth = device.PreferredBackBufferWidth;
        this.ScreenHeight = device.PreferredBackBufferHeight;

        var viewport = device.GraphicsDevice.Viewport;
        this.ScreenCenter = new Vector2(this.ScreenWidth / 2f, this.ScreenHeight / 2f);

        var widthScale = this.ScreenWidth / this.VirtualScreen.X;
        var heightScale = this.ScreenHeight / this.VirtualScreen.Y;

        this.ScreenScale = new Vector2(widthScale, heightScale);

        this.ScreenAspectRatio = new Vector2(widthScale / heightScale);
        this._scalingFactor = new Vector3(widthScale, heightScale, 1);
        this.Scale = Matrix.CreateScale(this._scalingFactor);

        device.ApplyChanges();
    }

    /// <summary>
    /// <para>Determines the draw scaling.</para>
    /// <para>Used to make the mouse scale correctly according to the virtual resolution,
    /// no matter the actual resolution.</para>
    /// <para>Example: 1920x1080 applied to 1280x800: new Vector2(1.5f, 1,35f)</para>
    /// </summary>
    /// <returns></returns>
    public Vector2 DetermineDrawScaling() {
        var x = this.ScreenWidth / this.VirtualScreen.X;
        var y = this.ScreenHeight / this.VirtualScreen.Y;
        return new Vector2(x, y);
    }

    public Vector2 GetMouseCoords() => Mouse.GetState().Position.ToVector2() / this.ScreenScale;

}