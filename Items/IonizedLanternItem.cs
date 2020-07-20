using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using aberration;
using Terraria.Enums;
using aberration.Tiles;
using Terraria;
using Microsoft.Xna.Framework;

namespace aberration.Items {
    class IonizedLanternItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ionized Lantern");
            Tooltip.SetDefault("It glows during the day.");
        }
        public override void HoldItem(Player player) {
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            float r = 0;
            float g = 0;
            float b = 0;
            shared.Lighting.ModifyLight((int)position.X/16, (int)position.Y/16, ref r, ref g, ref b);
            Lighting.AddLight(position, r, g, b);
        }
        public override void HoldStyle(Player player) {
            player.itemLocation += new Vector2(-4 * player.direction, 20);
        }
        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.holdStyle = 1;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.createTile = TileType<Tiles.IonizedLantern>();
            item.width = 10;
            item.height = 24;
            item.value = 500;
        }
    }
}