using Microsoft.Xna.Framework;
using TTG_Game.Models;

namespace TTG_Game.Controls; 

// Credits of original code: https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial014/Core/Camera.cs
public class Camera {

    public Matrix Transform { get; private set; }

    public void Follow(Sprite target) {
        var graphic = TTGGame.Instance.GraphicManager;

        var position = Matrix.CreateTranslation(
            -target.Position.X - (target.Rectangle.Width / 2),
            -target.Position.Y - (target.Rectangle.Height / 2),
            0
        );

        var offset = Matrix.CreateTranslation(
            graphic.ScreenWidth / 2,
            graphic.ScreenHeight / 2,
            0
        );

        this.Transform = position * graphic.Scale * offset;
    }

}