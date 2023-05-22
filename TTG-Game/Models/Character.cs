using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Utils.Extensions;

namespace TTG_Game.Models; 

public class Character {

    private static readonly Color PrimaryColor = new(48, 255, 0); // 30ff00
    private static readonly Color SecondaryColor = new(137, 255, 111); // 8aff6f
    private static readonly Color AccentColor = new(192, 255, 178); // c0ffb2

    private class ColorMapper {

        public Color From { get; }
        public Color To { get; }
        public float Threshold { get; }

        public ColorMapper(Color from, Color to, float threshold = 5f) {
            this.From = from;
            this.To = to;
            this.Threshold = threshold;
        }

    }

    private static void ChangeTextureColor(Texture2D texture, Color from, Color to, float threshold = 5f) {
        ChangeTextureColor(texture, new List<ColorMapper>() { new(from, to, threshold) });
    }

    private static void ChangeTextureColor(Texture2D texture, List<ColorMapper> colorMapping) {
        var data = new Color[texture.Width * texture.Height];
        texture.GetData(data);

        foreach (var color in colorMapping)
            for (var x = 0; x < data.Length; x++)
                // Calculate the Euclidean distance between the current pixel and the test color ff the distance is within a threshold, replace the color with the replacement color
                if (color.From.Distance(data[x]) < color.Threshold) // Adjust the threshold value as needed
                    data[x] = color.To;

        texture.SetData(data);
    }

    public Texture2D Idle { get; } = TTGGame.Instance.TextureManager.CharacterIdle;
    public List<Texture2D> Walk { get; } = TTGGame.Instance.TextureManager.CharacterWalk;
    public Texture2D Dead { get; } = TTGGame.Instance.TextureManager.CharacterDead;

    public Character(Color color) : this(color, color.Darker(.9f)) {}

    public Character(Color primaryColor, Color secondaryColor) {
        var colorMapping = new List<ColorMapper>() {
            new( PrimaryColor, primaryColor, 125f ),
            new( SecondaryColor, secondaryColor, 45f ),
            new( AccentColor, secondaryColor.Darker(.75f) )
        };

        ChangeTextureColor(this.Idle, colorMapping);
        this.Walk.ForEach(texture => ChangeTextureColor(texture, colorMapping));
        ChangeTextureColor(this.Dead, colorMapping);
    }

}