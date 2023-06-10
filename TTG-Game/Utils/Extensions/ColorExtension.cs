using System;
using Microsoft.Xna.Framework;

namespace TTG_Game.Utils.Extensions; 

public static class ColorExtension {

    public static Color GetFromSystemColor(System.Drawing.Color color) => new(color.R, color.G, color.B, color.A);

    // Calculate the Euclidean distance between two colors
    public static float Distance(this Color c1, Color c2) {
        var dr = c1.R - c2.R;
        var dg = c1.G - c2.G;
        var db = c1.B - c2.B;

        return (float) Math.Sqrt(dr * dr + dg * dg + db * db);
    }

    public static Color Darker(this Color color, float darknessFactor) {
        // Clamp the darkness factor between 0 and 1
        darknessFactor = MathHelper.Clamp(darknessFactor, 0f, 1f);

        // Calculate the darker color components
        var dr = (int) (color.R * darknessFactor);
        var dg = (int) (color.G * darknessFactor);
        var db = (int) (color.B * darknessFactor);

        // Create and return the darker color
        return new Color(dr, dg, db);
    }

}