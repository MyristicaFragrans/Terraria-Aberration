using aberration.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace aberration {
    class aberrantTile : GlobalTile {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem) {
            if( type == 3 ) { // grass
                if(WorldGen.genRand.Next(3) == 0) {
                    Item.NewItem(i * 16, j * 16, 1, 1, ItemID.GrassSeeds);
                }
                if(WorldGen.genRand.NextBool()) {
                    Item.NewItem(i * 16, j * 16, 1, 1, ItemType<plantfibre>());
                }
            }
        }
    }
}
