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

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16,  16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateWidth = 14;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.Origin = new Point16(0, 1);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(253, 221, 3), Language.GetText("MapObject.FloorLamp"));
        }

        public override bool CanPlace(int i, int j) => canPlacePublic(i, j);
        public static bool canPlacePublic(int i, int j) {
            if (Main.tile[i, j].active() || Main.tile[i, j - 1].active()) {//blocks are not free
                return false;
            }
            else {
                if (Main.tile[i + 1, j - 1].active() || Main.tile[i - 1, j - 1].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 2].active() || Main.tile[i, j - 1].wall != WallID.None) return true;
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
            shared.Lighting.ModifyLight(i, j, ref r, ref g, ref b);
        }
    }
}