using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace aberration.shared {
    class Lighting {
        internal static Color[] colors;
        internal static void init() {
            Texture2D day = GetTexture("aberration/shared/daynight");
            colors = new Color[day.Width];
            day.GetData(colors, 0, day.Width);
        }
        public static void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            Tile tile = Main.tile[i, j];
            Color light = colors[0];
            int brightness = 100 + (tile.liquid / 2); //gets darker in water
            if (Main.dayTime) {
                //pass
            } else if (Main.time < 5400) { // is before 1:30 past sunset
                int ratio = (int)((Main.time / 5400) * (colors.Length - 1));
                light = colors[ratio];
            } else if (Main.time > 27000) { // is after 1:30 till sunrise
                double tmptime = Main.time - 27000;
                int ratio = (colors.Length - 1) - (int)((tmptime / 5400) * (colors.Length - 1));
                light = colors[ratio];
            } else {
                light = colors[colors.Length - 1];
            }
            r = (float)light.R / brightness;
            g = (float)light.G / brightness;
            b = (float)light.B / brightness;
        }
    }
}
