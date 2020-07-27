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
        public override void AddRecipes() {
            ModRecipe rope = new ModRecipe(this);
            rope.AddIngredient(ModContent.ItemType<plantfibre>(), 5);
            rope.SetResult(ItemID.Rope);
            rope.AddRecipe();

            ModRecipe lantern = new ModRecipe(this);
            lantern.AddIngredient(ModContent.ItemType<IonizingMetalItem>());
            lantern.AddIngredient(ItemID.IronBar);
            lantern.AddTile(TileID.WorkBenches);
            lantern.SetResult(ModContent.ItemType<IonizedLanternItem>());
            lantern.AddRecipe();
        }
    }
}