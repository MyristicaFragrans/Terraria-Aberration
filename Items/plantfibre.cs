using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace aberration.Items {
    class plantfibre : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Plant Fibre");
            Tooltip.SetDefault("It is oddly strong...");
        }
        public override void SetDefaults() {
            base.SetDefaults();
            item.maxStack = 999;
        }
    }
}
