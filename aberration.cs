using aberration.Items;
using aberration.Tiles;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace aberration
{
	public class aberration : Mod {
        public aberration() {

        }
        public override void Load() {
            shared.Lighting.init();
            
        }
}