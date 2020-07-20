using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace aberration.Items {
    class IonizingMetalItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ionizing Metal");
            Tooltip.SetDefault("It glows during the day.\nIf there is such a thing anymore.");
        }
        public override void SetDefaults() {
            item.maxStack = 999;
            item.createTile = TileType<Tiles.IonizingMetal>();
        }
    }
}
