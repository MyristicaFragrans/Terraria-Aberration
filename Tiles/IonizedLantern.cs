using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;

namespace aberration.Tiles {
    class IonizedLantern : ModTile {
        public override void SetDefaults() {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = false;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(253, 221, 3), Language.GetText("MapObject.FloorLamp"));
        }

        public override bool CanPlace(int i, int j) {
            if(Main.tile[i,j].active() || Main.tile[i,j-1].active()) {//blocks are not free
                return false;
            } else {
                if (Main.tile[i + 1, j - 1].active() || Main.tile[i - 1, j - 1].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 2].active()) return true;
                else return false;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 16, 48, ItemType<Items.IonizedLanternItem>());
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
            if (i % 2 == 1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            Tile tile = Main.tile[i, j];
            Texture2D day = GetTexture("aberration/shared/daynight");
            Color[] colors = new Color[day.Width];
            day.GetData(colors);
            Color light = colors[0];
            int brightness = 100 + (tile.liquid/2); //gets darker in water
            if (Main.dayTime) {
                //pass
            } else if(Main.time < 5400) { // is before 1:30 past sunset
                int ratio = (int)((Main.time / 5400) * (day.Width-1));
                light = colors[ratio];
            } else if(Main.time > 27000) { // is after 1:30 till sunrise
                double tmptime = Main.time - 27000;
                int ratio = (day.Width-1) - (int)((tmptime / 5400) * (day.Width-1));
                light = colors[ratio];
            } else {
                light = colors[day.Width-1];
            }
            if (tile.frameX == 0) {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = (float)light.R / brightness;
                g = (float)light.G / brightness;
                b = (float)light.B / brightness;
            }
        }
    }
}