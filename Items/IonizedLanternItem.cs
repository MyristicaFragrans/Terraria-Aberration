using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using aberration;
using Terraria.Enums;

namespace aberration.Items {
    class IonizedLanternItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ionized Lantern");
            Tooltip.SetDefault("It glows during the day.");
        }
        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
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